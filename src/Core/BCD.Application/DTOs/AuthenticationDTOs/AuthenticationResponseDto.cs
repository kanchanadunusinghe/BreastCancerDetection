namespace BCD.Application.DTOs.AuthenticationDTOs;

public class AuthenticationResponseDto
{
    public bool IsSuccess { get; private set; }
    public string? Error { get; private set; }

    public string? DisplayName { get; private set; }
    public string? Token { get; private set; }
    public string? Email { get; private set; }
    public List<string>? Roles { get; private set; }
    public DateTime? ExpiresAt { get; private set; }
    public string? Message { get; private set; }
    public int? UserId { get; set; }

    private AuthenticationResponseDto() { }

    public static AuthenticationResponseDto Success(
        string displayName,
        string email,
        string token,
        List<string> roles,
        DateTime expiresAt,
        string message,
        int userId)
    {
        return new AuthenticationResponseDto
        {
            IsSuccess = true,
            DisplayName = displayName,
            Email = email,
            Token = token,
            Roles = roles,
            ExpiresAt = expiresAt,
            Message = message,
            UserId = userId
        };
    }

    public static AuthenticationResponseDto Failure(string error)
    {
        return new AuthenticationResponseDto
        {
            IsSuccess = false,
            Error = error
        };
    }
}
