using Logbook.Controllers;
using Logbook.Requests.Logbook;
using Logbook.Responses.Logbook;
using LogbookService.Dependencies.LogbookService;
using LogbookService.Records;
using LogbookServiceClient.Test.TestCaseSources;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace LogbookServiceClient.Test.Controllers;

public class LogbookControllerTest
{
    private Mock<ILogger<LogbookController>> LoggerMock { get; set; } = new();

    private Mock<ILogbookService> LogbookServiceMock { get; set; } = new();

    [SetUp]
    public void Setup()
    {
        this.LoggerMock = new();
        this.LogbookServiceMock = new();
    }

    [Test]
    [TestCaseSource(typeof(LoggedJumpTestCaseSources), nameof(LoggedJumpTestCaseSources.ListJumps_Valid))]
    public async Task TestListJumps_ReturnsListOfJumps(IEnumerable<LoggedJump> jumps)
    {
        // Arrange
        this.LogbookServiceMock.Setup(x => x.ListJumps(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
            .Returns(jumps)
            .Verifiable();

        LogbookController controller = new(this.LoggerMock.Object, this.LogbookServiceMock.Object);

        // Act
        var result = await controller.ListJumps(new ListJumpsRequest());

        // Verify
        this.LogbookServiceMock.Verify();

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOf<OkObjectResult>(result);
        Assert.IsInstanceOf<ListJumpsResponse>(((OkObjectResult)result).Value);
        Assert.That(jumps.Count(), Is.EqualTo((((OkObjectResult)result).Value as ListJumpsResponse)?.Jumps?.Count()));
        Assert.That((((OkObjectResult)result).Value as ListJumpsResponse)?.Jumps, Is.EqualTo(jumps));
    }

    [Test]
    [TestCase("Failed to list jumps", StatusCodes.Status500InternalServerError)]
    public async Task TestListJumps_ReturnsInternalServerErrorWithMessage(string message, int statusCode)
    {
        // Arrange
        this.LogbookServiceMock.Setup(x => x.ListJumps(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
            .Throws(new Exception("Random exception"))
            .Verifiable();

        LogbookController controller = new(this.LoggerMock.Object, this.LogbookServiceMock.Object);

        // Act
        var result = await controller.ListJumps(new ListJumpsRequest());

        // Verify
        this.LogbookServiceMock.Verify();

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOf<ObjectResult>(result);
        Assert.IsInstanceOf<ProblemDetails>(((ObjectResult)result).Value);
        Assert.That((((ObjectResult)result).Value as ProblemDetails)?.Detail, Is.EqualTo(message));
        Assert.That(((ObjectResult)result).StatusCode, Is.EqualTo(statusCode));
    }

    [Test]
    [TestCaseSource(typeof(LoggedJumpTestCaseSources), nameof(LoggedJumpTestCaseSources.LogJump_Valid))]
    public async Task TestLogJump_ReturnsOkWithLoggedJumpResponse(LoggedJump loggedJump)
    {
        // Arrange
        this.LogbookServiceMock.Setup(x => x.LogJump(It.Ref<LoggedJump>.IsAny))
            .Returns(loggedJump)
            .Verifiable();

        LogbookController controller = new(this.LoggerMock.Object, this.LogbookServiceMock.Object);

        // Act
        var result = await controller.LogJump(new LogJumpRequest());

        // Verify
        this.LogbookServiceMock.Verify();

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOf<OkObjectResult>(result);
        Assert.IsInstanceOf<LogJumpResponse>(((OkObjectResult)result).Value);
        Assert.That(((OkObjectResult)result).StatusCode, Is.EqualTo(StatusCodes.Status200OK));
        Assert.That((((OkObjectResult)result).Value as LogJumpResponse)?.LoggedJump, Is.EqualTo(loggedJump));
    }

    [Test]
    [TestCase("Failed to log jump", StatusCodes.Status500InternalServerError)]
    public async Task TestLogJump_ReturnsInternalServerErrorWithMessage(string message, int statusCode)
    {
        // Arrange
        this.LogbookServiceMock.Setup(x => x.LogJump(It.Ref<LoggedJump>.IsAny))
            .Throws(new Exception("Random exception"))
            .Verifiable();

        LogbookController controller = new(this.LoggerMock.Object, this.LogbookServiceMock.Object);

        // Act
        var result = await controller.LogJump(new LogJumpRequest());

        // Verify
        this.LogbookServiceMock.Verify();

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOf<ObjectResult>(result);
        Assert.IsInstanceOf<ProblemDetails>(((ObjectResult)result).Value);
        Assert.That((((ObjectResult)result).Value as ProblemDetails)?.Detail, Is.EqualTo(message));
        Assert.That(((ObjectResult)result).StatusCode, Is.EqualTo(statusCode));
    }

    [Test]
    [TestCaseSource(typeof(LoggedJumpTestCaseSources), nameof(LoggedJumpTestCaseSources.LogJump_Valid))]
    public async Task TestEditJump_ReturnsOkWithEditJumpResponse(LoggedJump loggedJump)
    {
        // Arrange
        this.LogbookServiceMock.Setup(x => x.EditJump(It.Ref<LoggedJump>.IsAny))
            .Returns(loggedJump)
            .Verifiable();

        LogbookController controller = new(this.LoggerMock.Object, this.LogbookServiceMock.Object);

        // Act
        var result = await controller.EditJump(new EditJumpRequest());

        // Verify
        this.LogbookServiceMock.Verify();

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOf<OkObjectResult>(result);
        Assert.IsInstanceOf<EditJumpResponse>(((OkObjectResult)result).Value);
        Assert.That(((OkObjectResult)result).StatusCode, Is.EqualTo(StatusCodes.Status200OK));
        Assert.That((((OkObjectResult)result).Value as EditJumpResponse)?.EditedJump, Is.EqualTo(loggedJump));
    }

    [Test]
    [TestCase("Failed to edit jump", StatusCodes.Status500InternalServerError)]
    public async Task TestEditJump_ReturnsInternalServerErrorWithMessage(string message, int statusCode)
    {
        // Arrange
        this.LogbookServiceMock.Setup(x => x.EditJump(It.Ref<LoggedJump>.IsAny))
            .Throws(new Exception("Random exception"))
            .Verifiable();

        LogbookController controller = new(this.LoggerMock.Object, this.LogbookServiceMock.Object);

        // Act
        var result = await controller.EditJump(new EditJumpRequest());

        // Verify
        this.LogbookServiceMock.Verify();

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOf<ObjectResult>(result);
        Assert.IsInstanceOf<ProblemDetails>(((ObjectResult)result).Value);
        Assert.That((((ObjectResult)result).Value as ProblemDetails)?.Detail, Is.EqualTo(message));
        Assert.That(((ObjectResult)result).StatusCode, Is.EqualTo(statusCode));
    }

    [Test]
    [TestCaseSource(typeof(LoggedJumpTestCaseSources), nameof(LoggedJumpTestCaseSources.LogJump_Valid))]
    public async Task TestDeleteJump_ReturnsOkWithDeleteJumpResponse(LoggedJump loggedJump)
    {
        // Arrange
        this.LogbookServiceMock.Setup(x => x.DeleteJump(It.Ref<LoggedJump>.IsAny))
            .Returns(loggedJump)
            .Verifiable();

        LogbookController controller = new(this.LoggerMock.Object, this.LogbookServiceMock.Object);

        // Act
        var result = await controller.DeleteJump(new DeleteJumpRequest());

        // Verify
        this.LogbookServiceMock.Verify();

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOf<OkObjectResult>(result);
        Assert.IsInstanceOf<DeleteJumpResponse>(((OkObjectResult)result).Value);
        Assert.That(((OkObjectResult)result).StatusCode, Is.EqualTo(StatusCodes.Status200OK));
        Assert.That((((OkObjectResult)result).Value as DeleteJumpResponse)?.DeletedJump, Is.EqualTo(loggedJump));
    }

    [Test]
    [TestCase("Failed to delete jump", StatusCodes.Status500InternalServerError)]
    public async Task TestDeleteJump_ReturnsInternalServerErrorWithMessage(string message, int statusCode)
    {
        // Arrange
        this.LogbookServiceMock.Setup(x => x.DeleteJump(It.Ref<LoggedJump>.IsAny))
            .Throws(new Exception("Random exception"))
            .Verifiable();

        LogbookController controller = new(this.LoggerMock.Object, this.LogbookServiceMock.Object);

        // Act
        var result = await controller.DeleteJump(new DeleteJumpRequest());

        // Verify
        this.LogbookServiceMock.Verify();

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOf<ObjectResult>(result);
        Assert.IsInstanceOf<ProblemDetails>(((ObjectResult)result).Value);
        Assert.That((((ObjectResult)result).Value as ProblemDetails)?.Detail, Is.EqualTo(message));
        Assert.That(((ObjectResult)result).StatusCode, Is.EqualTo(statusCode));
    }
}