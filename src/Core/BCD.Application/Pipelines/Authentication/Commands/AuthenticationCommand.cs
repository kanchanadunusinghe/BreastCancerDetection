using BCD.Application.Common.Interfaces;
using BCD.Application.DTOs.AuthenticationDTOs;
using BCD.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JwtRegisteredClaimNames = System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames;

namespace BCD.Application.Pipelines.Authentication.Commands;

public record AuthenticationCommand(AuthenticationDto authenticationDto)
    : IRequest<AuthenticationResponseDto>;


public class AuthenticationCommandHandler(
    IBCDContext bCDContext,
    IConfiguration configuration
    ) : IRequestHandler<AuthenticationCommand, AuthenticationResponseDto>
{
    public async Task<AuthenticationResponseDto> Handle(AuthenticationCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var user = await bCDContext.Users
                    .FirstOrDefaultAsync(
                    (user) => user.Email.ToLower().Trim() == request.authenticationDto.Email.ToLower().Trim());

            if (user is null)
            {
                return AuthenticationResponseDto.Failure("Invalid credentials");
            }

            var isValidPassword = BCrypt.Net.BCrypt.Verify(
               request.authenticationDto.Password,
               user.PasswordHash);

            if (!isValidPassword)
            {
                return AuthenticationResponseDto.Failure("Invalid credentials");
            }

            var configureTokenResponse = await ConfigureJwtToken(user);

            return configureTokenResponse;
        }
        catch (Exception)
        {
            return AuthenticationResponseDto.Failure("Error has been occured please try again.");
        }
    }

    private async Task<AuthenticationResponseDto> ConfigureJwtToken(User user)
    {
        if (user is null)
            return AuthenticationResponseDto
                .Failure("User not found.");

        var key = configuration["Tokens:Key"];
        var issuer = configuration["Tokens:Issuer"];

        if (string.IsNullOrWhiteSpace(key) || string.IsNullOrWhiteSpace(issuer))
            return AuthenticationResponseDto
                .Failure("JWT configuration is missing.");

        var roles = user.UserRoles?
            .Select(x => x.Role.Name)
            .ToList() ?? new List<string>();

        var expires = DateTime.UtcNow.AddHours(8);

        try
        {
            var securityKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(key));

            var credentials = new SigningCredentials(
                securityKey,
                SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Aud, "webapp"),
            new Claim("firstName", user.FirstName ?? string.Empty),
            new Claim("lastName", user.LastName ?? string.Empty),

            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var token = new JwtSecurityToken(
                issuer: issuer,
                claims: claims,
                expires: expires,
                signingCredentials: credentials
            );

            var tokenString = new JwtSecurityTokenHandler()
                .WriteToken(token);

            var response = AuthenticationResponseDto.Success(
                $"{user.FirstName} {user.LastName}".Trim(),
                user.Email,
                tokenString,
                roles,
                expires,
                "Login Success"
            );

            return response;
        }
        catch
        {
            return AuthenticationResponseDto
                .Failure("An unexpected error occurred while generating the token.");
        }
    }
}