using Amazon.DynamoDBv2.DataModel;

namespace LogbookService.Records;

[DynamoDBTable(nameof(SkydiverInfo))]
public class SkydiverInfo
{
    [DynamoDBHashKey]
    public string? Email { get; init; }

    [DynamoDBProperty]
    public string? FirstName { get; init; }

    [DynamoDBProperty]
    public string? LastName { get; init; }

    [DynamoDBProperty]
    public string? USPAMembershipNumber { get; init; }

    [DynamoDBProperty]
    public string? USPALicenseNumber { get; init; }
}