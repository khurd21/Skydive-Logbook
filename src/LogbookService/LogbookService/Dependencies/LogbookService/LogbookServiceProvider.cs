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

    public LoggedJump DeleteJump(in string id, in int jumpNumber)
    {
        LoggedJump jump = this.VerifyJumpExists(
            id: id,
            jumpNumber: jumpNumber);

        this.DynamoDBContext
                .DeleteAsync<LoggedJump>(id, jumpNumber)
                .Wait();

        return jump;
    }

    public LoggedJump EditJump(in LoggedJump jump)
    {
        this.VerifyJumpExists(
            id: jump.Id!,
            jumpNumber: jump.JumpNumber);

        this.DynamoDBContext
                .SaveAsync<LoggedJump>(jump)
                .Wait();

        return jump;
    }

    public IEnumerable<LoggedJump> ListJumps(in string id, in int from = 1, in int to = int.MaxValue)
    {
        return this.DynamoDBContext
                        .QueryAsync<LoggedJump>(
                            id,
                            QueryOperator.Between,
                            new object[] { from, to })
                        .GetRemainingAsync().Result
                        .OrderBy(jump => jump.JumpNumber);
    }

    public LoggedJump LogJump(in LoggedJump jump)
    {
        this.VerifyJumpDoesNotExist(
            id: jump.Id!,
            jumpNumber: jump.JumpNumber);

        this.DynamoDBContext
                .SaveAsync<LoggedJump>(jump)
                .Wait();

        return jump;
    }

    private LoggedJump VerifyJumpExists(in string id, in int jumpNumber)
    {
        LoggedJump jump = this.DynamoDBContext.LoadAsync<LoggedJump>(id, jumpNumber).Result;
        if (jump == null)
        {
            throw new JumpNotFoundException(id, jumpNumber);
        }
        return jump;
    }

    private void VerifyJumpDoesNotExist(in string id, in int jumpNumber)
    {
        if (this.DynamoDBContext.LoadAsync<LoggedJump>(id, jumpNumber).Result != null)
        {
            throw new JumpAlreadyExistsException(id, jumpNumber);
        }
    }
}