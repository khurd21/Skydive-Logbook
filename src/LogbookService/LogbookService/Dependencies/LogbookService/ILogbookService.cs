using LogbookService.Records;

namespace LogbookService.Dependencies.LogbookService;

public interface ILogbookService
{
    IEnumerable<LoggedJump> ListJumps();
    void LogJump(LoggedJump jump);
    void EditJump(LoggedJump jump);
    void DeleteJump(LoggedJump jump);
}