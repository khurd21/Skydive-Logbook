using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using LogbookService.Records;
using Microsoft.Extensions.Logging;

namespace LogbookService.Dependencies.LogbookService;

public class LogbookServiceProvider : ILogbookService
{
    private ILogger<LogbookServiceProvider> Logger { get; init; }

    private DynamoDBContext DynamoDBContext { get; init; }

    public LogbookServiceProvider(ILogger<LogbookServiceProvider> logger, DynamoDBContext dynamoDBContext)
    {
        this.Logger = logger;
        this.DynamoDBContext = dynamoDBContext;
    }

    public LoggedJump DeleteJump(in LoggedJump jump)
    {
        int hashKey = jump.USPAMembershipNumber;
        int rangeKey = jump.JumpNumber;

        this.DynamoDBContext
                .DeleteAsync<LoggedJump>(hashKey, rangeKey)
                .Wait();

        return jump;
    }

    public LoggedJump EditJump(in LoggedJump jump)
    {
        return this.LogJump(jump);
    }

    public IEnumerable<LoggedJump> ListJumps(in int uspaMembershipNumber, in int from, in int to)
    {
        return this.DynamoDBContext
                        .QueryAsync<LoggedJump>(
                            uspaMembershipNumber,
                            QueryOperator.Between,
                            new object[] { from, to })
                        .GetRemainingAsync().Result
                        .OrderBy(jump => jump.JumpNumber);
    }

    public LoggedJump LogJump(in LoggedJump jump)
    {
        this.Logger.LogInformation($"Logging jump {jump}");
        this.DynamoDBContext
                .SaveAsync<LoggedJump>(jump)
                .Wait();

        return jump;
    }
}