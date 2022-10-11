using System.ComponentModel.DataAnnotations;
using LogbookService.Records;
using LogbookService.Records.Enums;

namespace Logbook.Requests.Logbook;

public class EditJumpRequest
{
    [Required(ErrorMessage=$"{nameof(this.Jump)} is required")]
    public LoggedJump? Jump { get; init; }
}