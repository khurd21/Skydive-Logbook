using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using LogbookService.Records;
using Microsoft.Extensions.Logging;

namespace LogbookService.Dependencies.DynamoDB;

/*
 * DynamoDBTableManager
 * Descpription: A hard coded way to drop and re-create tables for debugging purposes.
 * Reference: https://docs.aws.amazon.com/sdkfornet/v3/apidocs/items/DynamoDBv2/MDynamoDBCreateTableCreateTableRequest.html
 * Enum Support: https://aws.amazon.com/blogs/developer/dynamodb-datamodel-enum-support/
 */
public class DynamoDBTableManager
{
    private AmazonDynamoDBClient Client { get; init; }

    private ILogger Logger { get; init; }

    public DynamoDBTableManager(AmazonDynamoDBClient dynamoDbClient, ILogger logger)
    {
        this.Client = dynamoDbClient;
        this.Logger = logger;
    }

    public void ReinitializeTables()
    {
        this.DeleteTables();
        this.CreateTables();
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
            Task<DeleteTableResponse>[] deleteTableTasks = new Task<DeleteTableResponse>[]
            {
                this.Client.DeleteTableAsync(new DeleteTableRequest { TableName = nameof(SkydiverInfo)}),
                this.Client.DeleteTableAsync(new DeleteTableRequest { TableName = nameof(LoggedJump)}),
            };
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
                AttributeName = nameof(SkydiverInfo.USPAMembershipNumber),
                AttributeType = "N",
            },
            new()
            {
                AttributeName = nameof(SkydiverInfo.Email),
                AttributeType = "S",
            },
            new()
            {
                AttributeName = nameof(SkydiverInfo.FirstName),
                AttributeType = "S",
            },
            new()
            {
                AttributeName = nameof(SkydiverInfo.LastName),
                AttributeType = "S",
            },
            new()
            {
                AttributeName = nameof(SkydiverInfo.USPALicenseNumber),
                AttributeType = "S",
            },
        };
        List<KeySchemaElement> keySchemaElements = new()
        {
            new()
            {
                AttributeName = nameof(SkydiverInfo.USPAMembershipNumber),
                KeyType = "HASH",
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
                AttributeName = nameof(LoggedJump.Id),
                AttributeType = "N",
            },
            new()
            {
                AttributeName = nameof(LoggedJump.JumpNumber),
                AttributeType = "N",
            },
            new()
            {
                AttributeName = nameof(LoggedJump.Date),
                AttributeType = "S",
            },
            new()
            {
                AttributeName = nameof(LoggedJump.Date),
                AttributeType = "S",
            },
            new()
            {
                AttributeName = nameof(LoggedJump.JumpCategory),
                AttributeType = "N",
            },
            new()
            {
                AttributeName = nameof(LoggedJump.Aircraft),
                AttributeType = "S",
            },
            new()
            {
                AttributeName = nameof(LoggedJump.Parachute),
                AttributeType = "S",
            },
            new()
            {
                AttributeName = nameof(LoggedJump.ParachuteSize),
                AttributeType = "N",
            },
            new()
            {
                AttributeName = nameof(LoggedJump.Dropzone),
                AttributeType = "S",
            },
            new()
            {
                AttributeName = nameof(LoggedJump.Description),
                AttributeType = "S",
            }
        };
        List<KeySchemaElement> keySchemaElements = new()
        {
            new()
            {
                AttributeName = nameof(LoggedJump.Id),
                KeyType = "HASH",
            },
            new()
            {
                AttributeName = nameof(LoggedJump.JumpNumber),
                KeyType = "RANGE",
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

    private IEnumerable<T> ConvertTaskToEnumerable<T>(Task<T>[] taskList)
    {
        List<T> response = new();
        foreach (Task<T> task in taskList)
        {
            response.Add(task.Result);
        }
        return response;
    }

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