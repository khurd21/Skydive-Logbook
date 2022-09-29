using LogbookService.Records;

namespace Logbook.Responses.Logbook;

public class ListJumpsResponse
{
    public IEnumerable<LoggedJump>? Jumps { get; init; }
}