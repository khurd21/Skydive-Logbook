using System.ComponentModel.DataAnnotations;
using LogbookService.Records;

namespace Logbook.Requests.Logbook;

public class LogJumpRequest
{
    [Required(ErrorMessage=$"{nameof(this.Jump)} is required")]
    public LoggedJump? Jump { get; init; }
}