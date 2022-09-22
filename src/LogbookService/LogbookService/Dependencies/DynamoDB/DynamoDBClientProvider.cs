using Amazon.DynamoDBv2;

namespace LogbookService.Dependencies.DynamoDB;

public class DynamoDBClientProvider : IServiceProvider
{
    private AmazonDynamoDBConfig Config { get; init; }

    public DynamoDBClientProvider(AmazonDynamoDBConfig dynamoDbConfig)
    {
        this.Config = dynamoDbConfig;
    }

    public object? GetService(Type serviceType)
    {
        return new AmazonDynamoDBClient(this.Config);
    }
}