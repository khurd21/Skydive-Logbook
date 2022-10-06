using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;

namespace LogbookService.Records.PropertyConverters;

/// <summary>
/// Adapted from: https://gist.github.com/jvwing/ac549044411b7953367e
/// For some reason, the DynamoDBContext doesn't support enums with flags.
/// </summary>
public class DynamoEnumConverter<TEnum> : IPropertyConverter
{
	public object FromEntry(DynamoDBEntry entry) => (TEnum)Enum.Parse(typeof(TEnum), entry.AsString());

	public DynamoDBEntry ToEntry(object value) => new Primitive(value?.ToString());
}