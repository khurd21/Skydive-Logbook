using Amazon.DynamoDBv2.DataModel;

namespace LogbookService.Records.JumpType;

[DynamoDBTable(nameof(JumpType))]
public class JumpType
{
    [DynamoDBHashKey]
    public int Id { get; init; }

    [DynamoDBProperty]
    public JumpCategory Category { get; init; }
}