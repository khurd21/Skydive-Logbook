using System.Configuration;
using Amazon.DynamoDBv2;
using Microsoft.Extensions.Configuration;

namespace LogbookService.Dependencies.DynamoDB;

public class DynamoDBConfigProvider : IServiceProvider
{
    private IConfiguration Configuration { get; init; }

    public DynamoDBConfigProvider(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public object? GetService(Type serviceType)
    {
        IConfigurationSection dynamoDbConfig = this.Configuration.GetSection("DynamoDb");
        bool runLocalDynamoDb = dynamoDbConfig.GetValue<bool>("LocalMode");
        if (runLocalDynamoDb == true)
        {
            return new AmazonDynamoDBConfig
            {
                ServiceURL = dynamoDbConfig.GetValue<string>("ServiceUrl"),
            };
        }
        return new AmazonDynamoDBConfig();
    }
}