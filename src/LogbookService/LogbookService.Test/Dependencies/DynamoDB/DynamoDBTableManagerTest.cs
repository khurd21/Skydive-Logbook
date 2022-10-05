using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using LogbookService.Dependencies.DynamoDB;
using Microsoft.Extensions.Logging;
using Moq;

namespace LogbookService.Test.Dependencies.DynamoDB;

public class DynamoDBTableManagerTest
{
    private Mock<AmazonDynamoDBClient> ClientMock { get; set; } = new();

    private Mock<ILogger<DynamoDBTableManager>> LoggerMock { get; set; } = new();

    [SetUp]
    public void Setup()
    {
        this.ClientMock = new();
        this.LoggerMock = new();
    }

    [Test]
    public void TestCreateTables_TablesDoNotExist()
    {
        // Arrange
        this.ClientMock.SetupSequence(x => x.CreateTableAsync(It.IsAny<CreateTableRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new CreateTableResponse()
            {
                TableDescription = new TableDescription()
                {
                    TableName = "TestName1",
                    TableStatus = TableStatus.ACTIVE,
                    ProvisionedThroughput = new ProvisionedThroughputDescription()
                    {
                        NumberOfDecreasesToday = 1,
                        ReadCapacityUnits = 1,
                        WriteCapacityUnits = 1,
                    },
                }
            })
            .ReturnsAsync(new CreateTableResponse()
            {
                TableDescription = new TableDescription()
                {
                    TableName = "TestName2",
                    TableStatus = TableStatus.ACTIVE,
                    ProvisionedThroughput = new ProvisionedThroughputDescription()
                    {
                        NumberOfDecreasesToday = 1,
                        ReadCapacityUnits = 1,
                        WriteCapacityUnits = 1,
                    },
                }
            });

        DynamoDBTableManager manager = new(this.ClientMock.Object, this.LoggerMock.Object);

        // Act
        var response = manager.CreateTables();

        // Verify
        this.ClientMock.Verify(x => x.CreateTableAsync(It.IsAny<CreateTableRequest>(), It.IsAny<CancellationToken>()), Times.Exactly(2));
        Assert.That(response.Count(), Is.EqualTo(2));
        Assert.That(response.ElementAt(0).TableDescription.TableName, Is.EqualTo("TestName1"));
        Assert.That(response.ElementAt(1).TableDescription.TableName, Is.EqualTo("TestName2"));
    }

    [Test]
    public void TestDeleteTables_TablesDoNotExist()
    {
        // Arrange
        this.ClientMock.Setup(x => x.ListTablesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ListTablesResponse()
            {
                TableNames = new List<string>()
                {
                    "TestName1",
                    "TestName2",
                }})
            .Verifiable();

        this.ClientMock.SetupSequence(x => x.DeleteTableAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new DeleteTableResponse()
            {
                TableDescription = new TableDescription()
                {
                    TableName = "TestName1",
                    TableStatus = TableStatus.ACTIVE,
                    ProvisionedThroughput = new ProvisionedThroughputDescription()
                    {
                        NumberOfDecreasesToday = 1,
                        ReadCapacityUnits = 1,
                        WriteCapacityUnits = 1,
                    },
                }
            })
            .ReturnsAsync(new DeleteTableResponse()
            {
                TableDescription = new TableDescription()
                {
                    TableName = "TestName2",
                    TableStatus = TableStatus.ACTIVE,
                    ProvisionedThroughput = new ProvisionedThroughputDescription()
                    {
                        NumberOfDecreasesToday = 1,
                        ReadCapacityUnits = 1,
                        WriteCapacityUnits = 1,
                    },
                }
            });

        DynamoDBTableManager manager = new(this.ClientMock.Object, this.LoggerMock.Object);

        // Act
        var response = manager.DeleteTables();

        // Verify
        this.ClientMock.Verify();
        this.ClientMock.Verify(x => x.ListTablesAsync(It.IsAny<CancellationToken>()), Times.Once);
        this.ClientMock.Verify(x => x.DeleteTableAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Exactly(2));
        Assert.That(response.Count(), Is.EqualTo(2));
        Assert.That(response.ElementAt(0).TableDescription.TableName, Is.EqualTo("TestName1"));
        Assert.That(response.ElementAt(1).TableDescription.TableName, Is.EqualTo("TestName2"));
    }

    [Test]
    public void TestDeleteTables_TablesDoNotExist_NoTables()
    {
        // Arrange
        this.ClientMock.Setup(x => x.ListTablesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ListTablesResponse()
            {
                TableNames = new List<string>(),
            })
            .Verifiable();

        DynamoDBTableManager manager = new(this.ClientMock.Object, this.LoggerMock.Object);

        // Act
        var response = manager.DeleteTables();

        // Verify
        this.ClientMock.Verify();
        this.ClientMock.Verify(x => x.ListTablesAsync(It.IsAny<CancellationToken>()), Times.Once);
        this.ClientMock.Verify(x => x.DeleteTableAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
        Assert.That(response.Count(), Is.EqualTo(0));
    }
}