using Amazon.DynamoDBv2.Model;

namespace LogbookService.Dependencies.DynamoDB;

public interface IDynamoDBTableManager
{
    IEnumerable<CreateTableResponse> CreateTables();
    IEnumerable<DeleteTableResponse> DeleteTables();
}