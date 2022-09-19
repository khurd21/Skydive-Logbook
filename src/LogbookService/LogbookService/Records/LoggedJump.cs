using Amazon.DynamoDBv2.DataModel;

namespace LogbookService.Records;

[DynamoDBTable(nameof(LoggedJump))]
public class LoggedJump
{
    [DynamoDBHashKey]
    public int Id { get; init; }

    [DynamoDBProperty]
    public JumpType? JumpType { get; init; }

    [DynamoDBProperty]
    public string? Description { get; init; }
}