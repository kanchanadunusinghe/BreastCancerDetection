using System.ComponentModel.DataAnnotations;

namespace BCD.Application.DTOs.AuthenticationDTOs;

public class AuthenticationDto
{
    [Required]
    [EmailAddress]
    [MaxLength(150)]
    public string Email { get; set; } = default!;

    [Required]
    [MinLength(6)]
    public string Password { get; set; } = default!;
}
