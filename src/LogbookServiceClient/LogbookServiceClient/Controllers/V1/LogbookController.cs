using System.Security.Claims;
using AutoMapper;
using Logbook.APIs;
using Logbook.Requests.Logbook;
using Logbook.Responses.Logbook;
using LogbookService.Records.Enums;
using LogbookService.Dependencies.LogbookService;
using LogbookService.Exceptions;
using LogbookService.Records;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Logbook.Controllers.V1;


[ApiController]
[ApiVersion("1.0")]
[Authorize]
[EnableCors]
[Route("api/v{version:ApiVersion}/logbook")]
public sealed class LogbookController : ControllerBase, ILogbookAPI
{
    private ILogger<LogbookController> Logger { get; init; }

    private ILogbookService LogbookService { get; init; }

    private IMapper Mapper { get; init; }

    public LogbookController(
        ILogger<LogbookController> logger, ILogbookService logbookService, IMapper mapper)
    {
        this.Logger = logger;
        this.LogbookService = logbookService;
        this.Mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(typeof(ListJumpsResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ObjectResult), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ListJumps([FromQuery] ListJumpsRequest request)
    {
        this.Logger.LogInformation($"{nameof(this.ListJumps)} called");
        try
        {
            string userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            IEnumerable<LoggedJump> jumps = this.LogbookService.ListJumps(
                id: userId,
                from: request.From,
                to: request.To);

            /*return await Task.FromResult(this.Ok(
                new ListJumpsResponse()
                {
                    Jumps = new List<LoggedJump>()
                    {
                        new()
                        {
                            Date = DateTime.Now,
                            JumpNumber = 1,
                            Dropzone = "Kapowsin",
                            JumpCategory = JumpCategory.FREEFLY,
                        },
                        new()
                        {
                            Date = DateTime.Now,
                            JumpNumber = 2,
                            JumpCategory = JumpCategory.BELLY,
                        },
                        new()
                        {
                            Date = DateTime.Now,
                            JumpNumber = 3,
                            JumpCategory = JumpCategory.BELLY,
                        },
                    }
                }
            ));*/
            return await Task.FromResult(this.Ok(
                new ListJumpsResponse() { Jumps = jumps }));
        }
        catch (SkydiverNotFoundException ex)
        {
            this.Logger.LogWarning(ex, $"{nameof(this.ListJumps)} failed");
            return await Task.FromResult(
                this.Problem(
                    detail: "Skydiver not found",
                    statusCode: StatusCodes.Status404NotFound));
        }
        catch (Exception ex)
        {
            this.Logger.LogWarning(ex, "Failed to list jumps");
            return await Task.FromResult(
                this.Problem(
                    detail: "Failed to list jumps",
                    statusCode: StatusCodes.Status500InternalServerError));
        }
        finally
        {
            this.Logger.LogInformation($"{nameof(this.ListJumps)} completed"); 
        }
    }

    [HttpPost]
    [ProducesResponseType(typeof(LogJumpResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ObjectResult), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> LogJump([FromBody] LogJumpRequest request)
    {
        this.Logger.LogInformation($"{nameof(this.LogJump)} called");
        try
        {
            request.Id = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            LoggedJump jump = this.Mapper.Map<LoggedJump>(request);
            LoggedJump loggedJump = this.LogbookService.LogJump(jump: jump);
            return await Task.FromResult(this.Ok(
                new LogJumpResponse() { LoggedJump = loggedJump }));
        }
        catch (SkydiverNotFoundException ex)
        {
            this.Logger.LogWarning(ex, $"{nameof(this.LogJump)} failed");
            return await Task.FromResult(
                this.Problem(
                    detail: "Skydiver not found",
                    statusCode: StatusCodes.Status404NotFound));
        }
        catch (JumpAlreadyExistsException ex)
        {
            this.Logger.LogWarning(ex, $"{nameof(this.LogJump)} failed");
            return await Task.FromResult(
                this.Problem(
                    detail: $"Jump with number {request.JumpNumber} already exists",
                    statusCode: StatusCodes.Status404NotFound));
        }
        catch (Exception ex)
        {
            this.Logger.LogError(ex, "Failed to log jump");
            return await Task.FromResult(
                this.Problem(
                    detail: "Failed to log jump",
                    statusCode: StatusCodes.Status500InternalServerError));
        }
        finally
        {
            this.Logger.LogInformation($"{nameof(this.LogJump)} completed"); 
        }
    }

    [HttpPut]
    [ProducesResponseType(typeof(EditJumpResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ObjectResult), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> EditJump([FromBody] EditJumpRequest request)
    {
        this.Logger.LogInformation($"{nameof(this.EditJump)} called");
        try
        {
            request.Id = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            LoggedJump jump = this.Mapper.Map<LoggedJump>(request);
            LoggedJump loggedJump = this.LogbookService.EditJump(jump: jump);
            return await Task.FromResult(this.Ok(
                new EditJumpResponse() { EditedJump = loggedJump }));
        }
        catch (SkydiverNotFoundException ex)
        {
            this.Logger.LogWarning(ex, $"{nameof(this.EditJump)} failed");
            return await Task.FromResult(
                this.Problem(
                    detail: "Skydiver not found",
                    statusCode: StatusCodes.Status404NotFound));
        }
        catch (JumpNotFoundException ex)
        {
            this.Logger.LogWarning(ex, $"{nameof(this.EditJump)} failed");
            return await Task.FromResult(
                this.Problem(
                    detail: $"Jump with number {request.JumpNumber} not found",
                    statusCode: StatusCodes.Status404NotFound));
        }
        catch (Exception ex)
        {
            this.Logger.LogError(ex, "Failed to edit jump");
            return await Task.FromResult(
                this.Problem(
                    detail: "Failed to edit jump",
                    statusCode: StatusCodes.Status500InternalServerError));
        }
        finally
        {
            this.Logger.LogInformation($"{nameof(this.EditJump)} completed"); 
        }
    }

    [HttpDelete]
    [ProducesResponseType(typeof(DeleteJumpResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ObjectResult), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteJump([FromQuery] DeleteJumpRequest request)
    {
        this.Logger.LogInformation($"{nameof(this.DeleteJump)} called");
        try
        {
            this.Logger.LogInformation($"Deleting jump {request.JumpNumber}");
            LoggedJump loggedJump = this.LogbookService.DeleteJump(
                id: this.User.FindFirstValue(ClaimTypes.NameIdentifier),
                jumpNumber: request.JumpNumber);

            return await Task.FromResult(this.Ok(
                new DeleteJumpResponse() { DeletedJump = loggedJump }));
        }
        catch (JumpNotFoundException ex)
        {
            this.Logger.LogWarning(ex, $"{nameof(this.DeleteJump)} failed");
            return await Task.FromResult(
                this.Problem(
                    detail: $"Jump with number {request.JumpNumber} not found",
                    statusCode: StatusCodes.Status404NotFound));
        }
        catch (Exception ex)
        {
            this.Logger.LogError(ex, "Failed to delete jump");
            return await Task.FromResult(
                this.Problem(
                    detail: "Failed to delete jump",
                    statusCode: StatusCodes.Status500InternalServerError));
        }
        finally
        {
            this.Logger.LogInformation($"{nameof(this.DeleteJump)} completed"); 
        }
    }
}
