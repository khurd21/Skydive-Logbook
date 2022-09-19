using Amazon.DynamoDBv2.DataModel;
using LogbookService.Records.Enums;

namespace LogbookService.Records;

[DynamoDBTable(nameof(JumpType))]
public class JumpType
{
    [DynamoDBHashKey]
    public int Id { get; init; }

    [DynamoDBProperty]
    public JumpCategory Category { get; init; }
}