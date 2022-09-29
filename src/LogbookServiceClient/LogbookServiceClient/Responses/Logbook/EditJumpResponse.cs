using LogbookService.Records;

namespace Logbook.Responses.Logbook;

public class EditJumpResponse
{
    public LoggedJump? EditedJump { get; init; }
}