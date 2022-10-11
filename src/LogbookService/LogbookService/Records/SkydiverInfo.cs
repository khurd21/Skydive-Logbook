using System.ComponentModel.DataAnnotations;
using Amazon.DynamoDBv2.DataModel;

namespace LogbookService.Records;

[DynamoDBTable(nameof(SkydiverInfo))]
public sealed class SkydiverInfo
{
    [DynamoDBHashKey]
    [Required(ErrorMessage=$"{nameof(USPAMembershipNumber)} is required")]
    public int USPAMembershipNumber { get; init; }

    [DynamoDBProperty]
    [RegularExpression(
        @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$",
        ErrorMessage="Email is not valid")]
    public string? Email { get; init; }

    [DynamoDBProperty]
    [RegularExpression(
        @"^[A-Z][a-zA-Z]*$",
        ErrorMessage=$"{nameof(this.FirstName)} must contain only letters and start with a capital letter")]
    public string? FirstName { get; init; }

    [DynamoDBProperty]
    [RegularExpression(
        @"^[A-Z][a-zA-Z]*$",
        ErrorMessage=$"{nameof(this.LastName)} must contain only letters and start with a capital letter")]
    public string? LastName { get; init; }

    [DynamoDBProperty]
    [RegularExpression(
        @"^[A-D]-\d+$",
        ErrorMessage=$"{nameof(this.USPALicenseNumber)} must be in the format `A-123456` or `B-123456` or `C-123456` or `D-123456`")]
    public string? USPALicenseNumber { get; init; }
}