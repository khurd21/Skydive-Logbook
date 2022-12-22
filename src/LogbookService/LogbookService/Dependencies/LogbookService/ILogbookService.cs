using LogbookService.Records;

namespace LogbookService.Dependencies.LogbookService;

public interface ILogbookService
{
    IEnumerable<LoggedJump> ListJumps(in string id, in int from, in int to);
    LoggedJump LogJump(in LoggedJump jump);
    LoggedJump EditJump(in LoggedJump jump);
    LoggedJump DeleteJump(in string id, in int jumpNumber);
}