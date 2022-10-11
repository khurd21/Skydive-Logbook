using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using LogbookService.Exceptions;
using LogbookService.Records;
using Microsoft.Extensions.Logging;

namespace LogbookService.Dependencies.LogbookService;

public class LogbookServiceProvider : ILogbookService
{
    private ILogger<LogbookServiceProvider> Logger { get; init; }

    private IDynamoDBContext DynamoDBContext { get; init; }

    public LogbookServiceProvider(ILogger<LogbookServiceProvider> logger, IDynamoDBContext dynamoDBContext)
    {
        this.Logger = logger;
        this.DynamoDBContext = dynamoDBContext;
    }

    public LoggedJump DeleteJump(in LoggedJump jump)
    {
        this.VerifyJumpExists(
            uspaMembershipNumber: jump.USPAMembershipNumber,
            jumpNumber: jump.JumpNumber);

        this.DynamoDBContext
                .DeleteAsync<LoggedJump>(jump.USPAMembershipNumber, jump.JumpNumber)
                .Wait();

        return jump;
    }

    public LoggedJump EditJump(in LoggedJump jump)
    {
        this.VerifySkydiverExists(
            uspaMembershipNumber: jump.USPAMembershipNumber);
        this.VerifyJumpExists(
            uspaMembershipNumber: jump.USPAMembershipNumber,
            jumpNumber: jump.JumpNumber);

        this.DynamoDBContext
                .SaveAsync<LoggedJump>(jump)
                .Wait();

        return jump;
    }

    public IEnumerable<LoggedJump> ListJumps(in int uspaMembershipNumber, in int from = 1, in int to = int.MaxValue)
    {
        this.VerifySkydiverExists(
            uspaMembershipNumber: uspaMembershipNumber);

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
        this.VerifySkydiverExists(
            uspaMembershipNumber: jump.USPAMembershipNumber);

        this.DynamoDBContext
                .SaveAsync<LoggedJump>(jump)
                .Wait();

        return jump;
    }

    private void VerifySkydiverExists(in int uspaMembershipNumber)
    {
        if (this.DynamoDBContext.LoadAsync<SkydiverInfo>(uspaMembershipNumber).Result == null)
        {
            throw new LogbookServiceException($"Skydiver {uspaMembershipNumber} does not exist");
        }
    }

    private void VerifyJumpExists(in int uspaMembershipNumber, in int jumpNumber)
    {
        if (this.DynamoDBContext.LoadAsync<LoggedJump>(uspaMembershipNumber, jumpNumber).Result == null)
        {
            throw new LogbookServiceException($"Jump {jumpNumber} does not exist for member {uspaMembershipNumber}");
        }
    }
}