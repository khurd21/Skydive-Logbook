using System.ComponentModel.DataAnnotations;

namespace Logbook.Requests.Logbook;

public class DeleteJumpRequest
{
    [Required(ErrorMessage=$"{nameof(this.USPAMembershipNumber)} is required")]
    public int USPAMembershipNumber { get; init; }

    [Required(ErrorMessage=$"{nameof(this.JumpNumber)} is required")]
    public int JumpNumber { get; init; }
}