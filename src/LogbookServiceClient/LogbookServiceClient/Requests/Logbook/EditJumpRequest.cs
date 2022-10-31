using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using LogbookService.Records.Enums;

namespace Logbook.Requests.Logbook;

public class EditJumpRequest
{
    [JsonIgnore] public string? Id { get; set; }

    [Required] public int JumpNumber { get; init; }

    public DateTime? Date { get; init; }

    public JumpCategory? JumpCategory { get; init; }

    public string? Aircraft { get; init; }

    public string? Parachute {get; init; }

    [Range(29, 500, ErrorMessage = $"{nameof(this.ParachuteSize)} must be between 29 and 500.")]
    public int? ParachuteSize { get; init; }

    public string? Dropzone { get; init; }

    public string? Description { get; init; }
}