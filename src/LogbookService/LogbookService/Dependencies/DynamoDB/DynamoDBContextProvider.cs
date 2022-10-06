using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;

namespace LogbookService.Dependencies.DynamoDB;

public class DynamoDBContextProvider : IServiceProvider
{
    private AmazonDynamoDBClient Client { get; init; }

    private DynamoDBContextConfig ContextConfig { get; init; }

    public DynamoDBContextProvider(AmazonDynamoDBClient client, DynamoDBContextConfig config)
    {
        this.Client = client;
        this.ContextConfig = config;
    }

    public object? GetService(Type serviceType)
    {
        return new DynamoDBContext(this.Client, this.ContextConfig);
    }
}