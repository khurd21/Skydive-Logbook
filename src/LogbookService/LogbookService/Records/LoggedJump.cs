using Amazon.DynamoDBv2.DataModel;
using LogbookService.Records.Enums;

namespace LogbookService.Records;

[DynamoDBTable(nameof(LoggedJump))]
public sealed class LoggedJump
{
    [DynamoDBHashKey]
    public int USPAMembershipNumber { get; init; }

    [DynamoDBRangeKey]
    public int JumpNumber { get; init; }

    [DynamoDBProperty]
    public DateTime Date { get; init; }

    [DynamoDBProperty]
    public JumpCategory? JumpCategory { get; init; }

    [DynamoDBProperty]    
    public string? Aircraft { get; init; }

    [DynamoDBProperty]
    public string? Parachute { get; init; }

    [DynamoDBProperty]
    public int ParachuteSize { get; init; }

    [DynamoDBProperty]
    public string? Dropzone { get; init; }

    [DynamoDBProperty]
    public string? Description { get; init; }
}