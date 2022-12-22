using System.ComponentModel.DataAnnotations;

namespace Logbook.Requests.Authentication;

public class RegisterRequest
{
    [Required(ErrorMessage=$"{nameof(this.Email)} is required")]
    public string? Email { get; init; }

    [Required(ErrorMessage=$"{nameof(this.Password)} is required")]
    public int USPAMembershipNumber { get; init; }

    [Required(ErrorMessage=$"{nameof(this.FirstName)} is required")]
    public string? FirstName { get; init; }

    [Required(ErrorMessage=$"{nameof(this.LastName)} is required")]
    public string? LastName { get; init; }

    [Required(ErrorMessage=$"{nameof(this.Password)} is required")]
    [RegularExpression(
        @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,15}$",
        ErrorMessage=$"{nameof(this.Password)} must be between 8 and 15 characters long and contain at least one lowercase letter, one uppercase letter, one number and one special character")]
    public string? Password { get; init; }

    [RegularExpression(
        @"^[A-D]-\d+$",
        ErrorMessage=$"{nameof(this.USPALicenseNumber)} must be in the format `A-123456` or `B-123456` or `C-123456` or `D-123456`")]
    public string? USPALicenseNumber { get; init; }
}