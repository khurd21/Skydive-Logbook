using LogbookService.Records;

namespace LogbookService.Dependencies.LogbookService;

public class LogbookServiceProvider : ILogbookService
{
    public LoggedJump DeleteJump(in LoggedJump jump)
    {
        throw new NotImplementedException();
    }

    public LoggedJump EditJump(in LoggedJump jump)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<LoggedJump> ListJumps(in int uspaMembershipNumber, in int from, in int to)
    {
        throw new NotImplementedException();
    }

    public LoggedJump LogJump(in LoggedJump jump)
    {
        throw new NotImplementedException();
    }
}