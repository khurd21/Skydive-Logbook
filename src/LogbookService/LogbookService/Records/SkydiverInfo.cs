using System.ComponentModel.DataAnnotations;
using Amazon.DynamoDBv2.DataModel;

namespace LogbookService.Records;

[DynamoDBTable(nameof(SkydiverInfo))]
public sealed class SkydiverInfo
{
    [DynamoDBHashKey]
    [Required(ErrorMessage=$"{nameof(this.Id)} is required")]
    public string? Id { get; init; }

    [DynamoDBProperty]
    [Required(ErrorMessage=$"{nameof(this.USPAMembershipNumber)} is required")]
    public int USPAMembershipNumber { get; init; }

    [DynamoDBProperty]
    [RegularExpression(
        @"^[A-D]-\d+$",
        ErrorMessage=$"{nameof(this.USPALicenseNumber)} must be in the format `A-123456` or `B-123456` or `C-123456` or `D-123456`")]
    public string? USPALicenseNumber { get; init; }
}