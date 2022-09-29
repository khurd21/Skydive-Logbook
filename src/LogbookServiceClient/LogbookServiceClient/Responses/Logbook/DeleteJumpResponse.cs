using LogbookService.Records;

namespace Logbook.Responses.Logbook;

public class DeleteJumpResponse
{
    public LoggedJump? DeletedJump { get; init; }
}