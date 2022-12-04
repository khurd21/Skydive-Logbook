using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Amazon.DynamoDBv2.DataModel;
using LogbookService.Records.Enums;

namespace LogbookService.Records;

[DynamoDBTable(nameof(LoggedJump))]
public sealed class LoggedJump
{
    [DynamoDBHashKey]
    [Required]
    [JsonIgnore]
    public string? Id { get; init; }

    [DynamoDBRangeKey]
    [Required]
    public int JumpNumber { get; init; }

    [DynamoDBProperty]
    public DateTime? Date { get; init; }

    [DynamoDBProperty]
    public JumpCategory? JumpCategory { get; init; }

    [DynamoDBProperty]
    public string? Aircraft { get; init; }

    [DynamoDBProperty]
    public int? Altitude { get; init; }

    [DynamoDBProperty]
    public string? Parachute { get; init; }

    [DynamoDBProperty]
    [Range(29, 500, ErrorMessage = $"{nameof(this.ParachuteSize)} must be greater than 29 and less than 500")]
    public int? ParachuteSize { get; init; }

    [DynamoDBProperty]
    public string? Dropzone { get; init; }

    [DynamoDBProperty]
    public string? Description { get; init; }
}