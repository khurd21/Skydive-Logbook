using System.ComponentModel.DataAnnotations;

namespace Logbook.Requests.Authentication;

public class AuthenticateRequest
{
    [Required(ErrorMessage=$"{nameof(this.Email)} is required")]
    public string? Email { get; init; }

    [Required(ErrorMessage=$"{nameof(this.Password)} is required")]
    public string? Password { get; init; }
}