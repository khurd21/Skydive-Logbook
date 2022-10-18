using System.Linq;
using System.Reflection;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using LogbookService.Dependencies.LogbookService;
using LogbookService.Exceptions;
using LogbookService.Records;
using Microsoft.Extensions.Logging;
using Moq;

namespace LogbookService.Test.Dependencies.LogbookService;

public class LogbookServiceProviderTest
{
    private Mock<ILogger<LogbookServiceProvider>> LoggerMock { get; set; } = new();

    private Mock<IDynamoDBContext> DynamoDBContextMock { get; set; } = new();

    private LogbookServiceProvider LogbookServiceProvider { get; set; } = null!;

    [SetUp]
    public void Setup()
    {
        this.LoggerMock = new();
        this.DynamoDBContextMock = new();
        this.LogbookServiceProvider = new(
            this.LoggerMock.Object,
            this.DynamoDBContextMock.Object);
    }

    [Test]
    public void DeleteJump_ReturnsValidLoggedJump()
    {
        // Arrange
        LoggedJump jump = new()
        {
            USPAMembershipNumber = 123456,
            JumpNumber = 1,
        };

        this.DynamoDBContextMock
            .Setup(context => context.DeleteAsync<LoggedJump>(It.IsAny<int>(), It.IsAny<int>(), It.Ref<CancellationToken>.IsAny))
            .Returns(Task.CompletedTask)
            .Verifiable();

        this.DynamoDBContextMock
            .Setup(
                context => context.LoadAsync<LoggedJump>(
                    It.IsAny<int>(),
                    It.IsAny<int>(),
                    It.Ref<CancellationToken>.IsAny))
            .Returns(Task.FromResult(new LoggedJump()))
            .Verifiable();

        // Act
        var result = this.LogbookServiceProvider.DeleteJump(jump);

        // Verify
        this.DynamoDBContextMock.Verify();

        // Assert
        Assert.NotNull(result);
        Assert.That(result, Is.EqualTo(jump));
    }

    [Test]
    public void DeleteJump_ThrowsJumpNotFoundException()
    {
        // Arrange
        this.DynamoDBContextMock
            .Setup(
                context => context.LoadAsync<LoggedJump>(
                    It.IsAny<int>(),
                    It.IsAny<int>(),
                    It.Ref<CancellationToken>.IsAny))
            .Returns(Task.FromResult<LoggedJump>(null!))
            .Verifiable();

        // Act
        Assert.Throws<JumpNotFoundException>(() => this.LogbookServiceProvider.DeleteJump(new LoggedJump()));

        // Verify
        this.DynamoDBContextMock.Verify();
    }

    [Test]
    public void EditJump_ReturnsLoggedJump()
    {
        // Arrange
        LoggedJump jump = new()
        {
            USPAMembershipNumber = 123456,
            JumpNumber = 1,
        };

        this.DynamoDBContextMock
            .Setup(context => context.SaveAsync<LoggedJump>(It.IsAny<LoggedJump>(), It.Ref<CancellationToken>.IsAny))
            .Returns(Task.CompletedTask)
            .Verifiable();

        this.DynamoDBContextMock
            .Setup(
                context => context.LoadAsync<LoggedJump>(
                    It.IsAny<int>(),
                    It.IsAny<int>(),
                    It.Ref<CancellationToken>.IsAny))
            .Returns(Task.FromResult(new LoggedJump()))
            .Verifiable();

        // Act
        var result = this.LogbookServiceProvider.EditJump(jump);

        // Verify
        this.DynamoDBContextMock.Verify();

        // Assert
        Assert.NotNull(result);
        Assert.That(result, Is.EqualTo(jump));
    }

    [Test]
    public void EditJump_ThrowsJumpNotFoundException()
    {
        // Arrange
        this.DynamoDBContextMock
            .Setup(
                context => context.LoadAsync<LoggedJump>(
                    It.IsAny<int>(),
                    It.IsAny<int>(),
                    It.Ref<CancellationToken>.IsAny))
            .Returns(Task.FromResult<LoggedJump>(null!))
            .Verifiable();

        // Act
        Assert.Throws<JumpNotFoundException>(() => this.LogbookServiceProvider.EditJump(new LoggedJump()));

        // Verify
        this.DynamoDBContextMock.Verify();
    }

    [Test]
    public void ListJumps_ReturnsListLoggedJumps()
    {
        // Arrange
        this.DynamoDBContextMock
            .Setup(
                context => context.QueryAsync<LoggedJump>(
                    It.IsAny<int>(),
                    It.IsAny<QueryOperator>(),
                    It.IsAny<IEnumerable<object>>(),
                    It.IsAny<DynamoDBOperationConfig>())
                .GetRemainingAsync(It.Ref<CancellationToken>.IsAny))
            .Returns(Task.FromResult(new List<LoggedJump>()))
            .Verifiable();

        // Act
        var result = this.LogbookServiceProvider.ListJumps(123456);

        // Verify
        this.DynamoDBContextMock.Verify();

        // Assert
        Assert.NotNull(result);
        Assert.That(result, Is.EqualTo(new List<LoggedJump>()));
    }
}