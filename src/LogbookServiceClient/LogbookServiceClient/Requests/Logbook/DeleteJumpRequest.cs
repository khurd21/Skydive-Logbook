using System.ComponentModel.DataAnnotations;

namespace Logbook.Requests.Logbook;

public sealed class DeleteJumpRequest
{
    [Required(ErrorMessage=$"{nameof(this.JumpNumber)} is required")]
    public int JumpNumber { get; init; }
}