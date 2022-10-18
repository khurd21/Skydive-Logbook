using System.ComponentModel.DataAnnotations;
using LogbookService.Records;

namespace Logbook.Requests.Logbook;

public class LogJumpRequest
{

    // TODO: Remove Logged Jump and use all attributes besides USPAMembershipNumber
    // TODO: Grab USPAMembership number from the JWT
    [Required(ErrorMessage=$"{nameof(this.Jump)} is required")]
    public LoggedJump? Jump { get; init; }
}