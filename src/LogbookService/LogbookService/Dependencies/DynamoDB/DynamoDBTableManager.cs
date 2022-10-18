using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using LogbookService.Records;
using Microsoft.Extensions.Logging;

namespace LogbookService.Dependencies.DynamoDB;

/*
 * DynamoDBTableManager
 * Description: A hard coded way to drop and re-create tables for debugging purposes.
 * Reference: https://docs.aws.amazon.com/sdkfornet/v3/apidocs/items/DynamoDBv2/MDynamoDBCreateTableCreateTableRequest.html
 * Enum Support: https://aws.amazon.com/blogs/developer/dynamodb-datamodel-enum-support/
 */
public sealed class DynamoDBTableManager : IDynamoDBTableManager
{
    private AmazonDynamoDBClient Client { get; init; }

    private ILogger<DynamoDBTableManager> Logger { get; init; }

    private IEnumerable<string> TableNames { get; init; }

    public DynamoDBTableManager(AmazonDynamoDBClient dynamoDbClient, ILogger<DynamoDBTableManager> logger)
    {
        this.Client = dynamoDbClient;
        this.Logger = logger;
        this.TableNames = new List<string>
        {
            nameof(SkydiverInfo),
            nameof(LoggedJump),
        };
    }

    public IEnumerable<CreateTableResponse> CreateTables()
    {
        try
        {
            Task<CreateTableResponse>[] createTableTasks = new Task<CreateTableResponse>[]
            {
                this.CreateSkydiverInfoTableAsync(),
                this.CreateLoggedJumpTableAsync(),
            };
            Task task = Task.WhenAll(createTableTasks);
            task.Wait();
            this.AssertAndLogTaskResult(nameof(this.CreateTables), task);
            return this.ConvertTaskToEnumerable(createTableTasks);
        }
        catch (Exception exc)
        {
            throw new SystemException("Error creating tables.", exc);
        }
    }

    public IEnumerable<DeleteTableResponse> DeleteTables()
    {
        try
        {
            ListTablesResponse listTablesResponse = this.Client.ListTablesAsync().Result;
            IEnumerable<string> tableNames = listTablesResponse.TableNames;
            Task<DeleteTableResponse>[] deleteTableTasks = tableNames.Select(tableName => this.Client.DeleteTableAsync(tableName)).ToArray();
            Task task = Task.WhenAll(deleteTableTasks);
            task.Wait();
            this.AssertAndLogTaskResult(nameof(this.DeleteTables), task);
            return this.ConvertTaskToEnumerable(deleteTableTasks);
        }
        catch (Exception exc)
        {
            throw new SystemException("Error deleting tables.", exc);
        }
    }

    private async Task<CreateTableResponse> CreateSkydiverInfoTableAsync()
    {
        List<AttributeDefinition> attributeDefinitions = new()
        {
            new()
            {
                AttributeName = nameof(SkydiverInfo.Email),
                AttributeType = ScalarAttributeType.S,
            },
        };
        List<KeySchemaElement> keySchemaElements = new()
        {
            new()
            {
                AttributeName = nameof(SkydiverInfo.Email),
                KeyType = KeyType.HASH,
            },
        };
        ProvisionedThroughput provisionedThroughput = new()
        {
            ReadCapacityUnits = 5,
            WriteCapacityUnits = 6,
        };
        CreateTableRequest request = new()
        {
            TableName = nameof(SkydiverInfo),
            AttributeDefinitions = attributeDefinitions,
            KeySchema = keySchemaElements,
            ProvisionedThroughput = provisionedThroughput,
        };
        return await this.CreateAndLogTable(request);
    }

    private async Task<CreateTableResponse> CreateLoggedJumpTableAsync()
    {
        List<AttributeDefinition> attributeDefinitions = new()
        {
            new()
            {
                AttributeName = nameof(LoggedJump.USPAMembershipNumber),
                AttributeType = ScalarAttributeType.N,
            },
            new()
            {
                AttributeName = nameof(LoggedJump.JumpNumber),
                AttributeType = ScalarAttributeType.N,
            },
        };
        List<KeySchemaElement> keySchemaElements = new()
        {
            new()
            {
                AttributeName = nameof(LoggedJump.USPAMembershipNumber),
                KeyType = KeyType.HASH,
            },
            new()
            {
                AttributeName = nameof(LoggedJump.JumpNumber),
                KeyType = KeyType.RANGE,
            }
        };
        ProvisionedThroughput provisionedThroughput = new()
        {
            ReadCapacityUnits = 5,
            WriteCapacityUnits = 6,
        };

        CreateTableRequest request = new()
        {
            TableName = nameof(LoggedJump),
            AttributeDefinitions = attributeDefinitions,
            KeySchema = keySchemaElements,
            ProvisionedThroughput = provisionedThroughput,
        };
        return await this.CreateAndLogTable(request);
    }

    private async Task<CreateTableResponse> CreateAndLogTable(CreateTableRequest request)
    {
        CreateTableResponse response = await this.Client.CreateTableAsync(request);
        this.LogTableCreated(response.TableDescription);
        return response;
    }

    private IEnumerable<T> ConvertTaskToEnumerable<T>(Task<T>[] taskList) => taskList.Select(task => task.Result);

    private void LogTableCreated(TableDescription tableDescription)
    {
        this.Logger.LogInformation($"Table: {tableDescription.TableName}\t Status: {tableDescription.TableStatus}" +
                                    $" \t ReadCapacityUnits: {tableDescription.ProvisionedThroughput.ReadCapacityUnits}" +
                                    $" \t WriteCapacityUnits: {tableDescription.ProvisionedThroughput.WriteCapacityUnits}");
    }

    private void AssertAndLogTaskResult(string tableName, Task task)
    {
        string message = $"{tableName} - Status: {task.Status}";
        if (task.Status == TaskStatus.Canceled)
        {
            this.Logger.LogError(message);
            throw new TaskCanceledException(message);
        }
        else if (task.Status == TaskStatus.Faulted)
        {
            this.Logger.LogError(message);
            throw new TaskSchedulerException(message);
        }
        else
        {
            this.Logger.LogInformation(message);
        }
    }
}