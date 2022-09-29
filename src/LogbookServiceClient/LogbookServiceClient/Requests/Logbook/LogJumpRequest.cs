using LogbookService.Records.Enums;

namespace Logbook.Requests.Logbook;

public class LogJumpRequest
{
    public int USPAMembershipNumber { get; init; }
    // JumpNumber might be generated by the server -- depending on the previous jump number
    // public int JumpNumber { get; init; }
    public DateTime? Date { get; init; }
    public JumpCategory? JumpCategory { get; init; }
    public string? Aircraft { get; init; }
    public string? Parachute { get; init; }
    public int? ParachuteSize { get; init; }
    public string? Dropzone { get; init; }
    public string? Description { get; init; }
}