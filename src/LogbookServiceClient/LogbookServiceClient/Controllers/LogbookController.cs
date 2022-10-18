using Logbook.APIs;
using Logbook.Requests.Logbook;
using Logbook.Responses.Logbook;
using LogbookService.Dependencies.LogbookService;
using LogbookService.Exceptions;
using LogbookService.Records;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Logbook.Controllers;


[ApiController]
[Authorize]
[Route("logbook")]
public sealed class LogbookController : ControllerBase, ILogbookAPI
{
    private ILogger<LogbookController> Logger { get; init; }

    private ILogbookService LogbookService { get; init; }

    public LogbookController(ILogger<LogbookController> logger, ILogbookService logbookService)
    {
        this.Logger = logger;
        this.LogbookService = logbookService;
    }

    [HttpGet("listjumps")]
    [ProducesResponseType(typeof(ListJumpsResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ObjectResult), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ListJumps([FromQuery] ListJumpsRequest request)
    {
        this.Logger.LogInformation($"{nameof(this.ListJumps)} called");
        try
        {
            IEnumerable<LoggedJump> jumps = this.LogbookService.ListJumps(
                uspaMembershipNumber: request.USPAMembershipNumber,
                from: request.From,
                to: request.To);

            return await Task.FromResult(this.Ok(
                new ListJumpsResponse() { Jumps = jumps }));
        }
        catch (SkydiverNotFoundException ex)
        {
            this.Logger.LogWarning(ex, $"{nameof(this.ListJumps)} failed");
            return await Task.FromResult(
                this.Problem(
                    detail: $"Skydiver with USPA membership number {request.USPAMembershipNumber} not found",
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

    [HttpPost("logjump")]
    [ProducesResponseType(typeof(LogJumpResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ObjectResult), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> LogJump([FromBody] LogJumpRequest request)
    {
        this.Logger.LogInformation($"{nameof(this.LogJump)} called");
        try
        {
            LoggedJump loggedJump = this.LogbookService.LogJump(jump: request.Jump!);
            return await Task.FromResult(this.Ok(
                new LogJumpResponse() { LoggedJump = loggedJump }));
        }
        catch (SkydiverNotFoundException ex)
        {
            this.Logger.LogWarning(ex, $"{nameof(this.LogJump)} failed");
            return await Task.FromResult(
                this.Problem(
                    detail: $"Skydiver with USPA membership number {request.Jump!.USPAMembershipNumber} not found",
                    statusCode: StatusCodes.Status404NotFound));
        }
        catch (JumpAlreadyExistsException ex)
        {
            this.Logger.LogWarning(ex, $"{nameof(this.LogJump)} failed");
            return await Task.FromResult(
                this.Problem(
                    detail: $"Jump with number {request.Jump!.JumpNumber} already exists",
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

    [HttpPut("editjump")]
    [ProducesResponseType(typeof(EditJumpResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ObjectResult), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> EditJump([FromBody] EditJumpRequest request)
    {
        this.Logger.LogInformation($"{nameof(this.EditJump)} called");
        try
        {
            LoggedJump loggedJump = this.LogbookService.EditJump(jump: request.Jump!);
            return await Task.FromResult(this.Ok(
                new EditJumpResponse() { EditedJump = loggedJump }));
        }
        catch (SkydiverNotFoundException ex)
        {
            this.Logger.LogWarning(ex, $"{nameof(this.EditJump)} failed");
            return await Task.FromResult(
                this.Problem(
                    detail: $"Skydiver with USPA membership number {request.Jump!.USPAMembershipNumber} not found",
                    statusCode: StatusCodes.Status404NotFound));
        }
        catch (JumpNotFoundException ex)
        {
            this.Logger.LogWarning(ex, $"{nameof(this.EditJump)} failed");
            return await Task.FromResult(
                this.Problem(
                    detail: $"Jump with number {request.Jump!.JumpNumber} not found",
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

    [HttpDelete("deletejump")]
    [ProducesResponseType(typeof(DeleteJumpResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ObjectResult), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteJump([FromQuery] DeleteJumpRequest request)
    {
        this.Logger.LogInformation($"{nameof(this.DeleteJump)} called");
        try
        {
            LoggedJump loggedJump = this.LogbookService.DeleteJump(new LoggedJump()
            {
                USPAMembershipNumber = request.USPAMembershipNumber,
                JumpNumber = request.JumpNumber,
            });

            return await Task.FromResult(this.Ok(
                new DeleteJumpResponse() { DeletedJump = loggedJump }));
        }
        catch (JumpNotFoundException ex)
        {
            this.Logger.LogWarning(ex, $"{nameof(this.DeleteJump)} failed");
            return await Task.FromResult(
                this.Problem(
                    detail: $"Jump with number {request.JumpNumber} from member {request.USPAMembershipNumber} not found",
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
