using Amazon.DynamoDBv2.DataModel;

namespace LogbookService.Dependencies.DynamoDB;

public class DynamoDBContextConfigProvider : IServiceProvider
{
    /// <summary>
    /// When retrieving data using the Load, Query, or Scan operations,
    /// you can add this optional parameter to request the latest
    /// values for the data.
    /// </summary>
    private bool ConsistentRead { get; } = true;

    /// <summary>
    /// This parameter informs DynamoDBContext to ignore null values
    /// on attributes during a Save operation. If this parameter is
    /// false (or if it is not set), then a null value is interpreted
    /// as a directive to delete the specific attribute. 
    /// </summary>
    private bool IgnoreNullValues { get; } = true;

    /// <summary>
    /// This parameter informs DynamoDBContext not to compare versions
    /// when saving or deleting an item. For more information about
    /// versioning, see Optimistic locking using a version number
    /// with DynamoDB using the AWS SDK for .NET object persistence model. 
    /// <link>
    /// https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/DynamoDBContext.VersionSupport.html
    /// </link>
    /// </summary>
    private bool SkipVersionCheck { get; } = false;

    /// <summary>
    /// Prefixes all table names with a specific string.
    /// If this parameter is null (or if it is not set),
    /// then no prefix is used.
    /// </summary>
    private string? TableNamePrefix { get; } = null;

    public object? GetService(Type serviceType)
    {
        return new DynamoDBContextConfig()
        {
            ConsistentRead = this.ConsistentRead,
            IgnoreNullValues = this.IgnoreNullValues,
            SkipVersionCheck = this.SkipVersionCheck,
            TableNamePrefix = this.TableNamePrefix,
        };
    }
}