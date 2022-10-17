using Amazon.DynamoDBv2.DataModel;
using Microsoft.Extensions.Configuration;

namespace LogbookService.Dependencies.DynamoDB;

public class DynamoDBContextConfigProvider : IServiceProvider
{
    /// <summary>
    /// When retrieving data using the Load, Query, or Scan operations,
    /// you can add this optional parameter to request the latest
    /// values for the data.
    /// </summary>
    private bool ConsistentRead { get; init; }

    /// <summary>
    /// This parameter informs DynamoDBContext to ignore null values
    /// on attributes during a Save operation. If this parameter is
    /// false (or if it is not set), then a null value is interpreted
    /// as a directive to delete the specific attribute. 
    /// </summary>
    private bool IgnoreNullValues { get; init; }

    /// <summary>
    /// This parameter informs DynamoDBContext not to compare versions
    /// when saving or deleting an item. For more information about
    /// versioning, see Optimistic locking using a version number
    /// with DynamoDB using the AWS SDK for .NET object persistence model. 
    /// <link>
    /// https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/DynamoDBContext.VersionSupport.html
    /// </link>
    /// </summary>
    private bool SkipVersionCheck { get; init; }

    /// <summary>
    /// Prefixes all table names with a specific string.
    /// If this parameter is null (or if it is not set),
    /// then no prefix is used.
    /// </summary>
    private string? TableNamePrefix { get; init; }

    private IConfigurationSection Configuration { get; init; }

    public DynamoDBContextConfigProvider(IConfiguration configuration)
    {
        this.Configuration = configuration.GetSection("DynamoDb");

        this.ConsistentRead = this.Configuration.GetValue<bool>("ConsistentRead");
        this.IgnoreNullValues = this.Configuration.GetValue<bool>("IgnoreNullValues");
        this.SkipVersionCheck = this.Configuration.GetValue<bool>("SkipVersionCheck");
        this.TableNamePrefix = this.Configuration.GetValue<string>("TableNamePrefix");
    }

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