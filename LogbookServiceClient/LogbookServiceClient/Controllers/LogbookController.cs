using Logbook.APIs;
using LogbookService.Dependencies;
using LogbookService.Records;
using Microsoft.AspNetCore.Mvc;

namespace Logbook.Controllers;


[ApiController]
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
    public async Task<IActionResult> ListJumps()
    {
        this.Logger.LogInformation($"{nameof(this.ListJumps)} called");
        try
        {
            IEnumerable<LoggedJump> jumps = this.LogbookService.ListJumps();
            return await Task.FromResult(this.Ok(jumps));
        }
        catch (Exception ex)
        {
            this.Logger.LogError(ex, "Failed to list jumps");
            return this.StatusCode(500, "Unkown error. Failed to list jumps");
        }
        finally
        {
            this.Logger.LogInformation($"{nameof(this.ListJumps)} completed"); 
        }
    }

    [HttpPost("logjump")]
    public async Task<IActionResult> LogJump()
    {
        this.Logger.LogInformation($"{nameof(this.LogJump)} called");
        try
        {
            this.LogbookService.LogJump(new LoggedJump());
            return await Task.FromResult(this.Ok());
        }
        catch (Exception ex)
        {
            this.Logger.LogError(ex, "Failed to log jump");
            return this.StatusCode(500, "Unkown error. Failed to log jump");
        }
        finally
        {
            this.Logger.LogInformation($"{nameof(this.LogJump)} completed"); 
        }
    }

    [HttpPut("editjump")]
    public async Task<IActionResult> EditJump()
    {
        this.Logger.LogInformation($"{nameof(this.EditJump)} called");
        try
        {
            this.LogbookService.EditJump(new LoggedJump());
            return await Task.FromResult(this.Ok());
        }
        catch (Exception ex)
        {
            this.Logger.LogError(ex, "Failed to edit jump");
            return this.StatusCode(500, "Unkown error. Failed to edit jump");
        }
        finally
        {
            this.Logger.LogInformation($"{nameof(this.EditJump)} completed"); 
        }
    }

    [HttpDelete("deletejump")]
    public async Task<IActionResult> DeleteJump()
    {
        this.Logger.LogInformation($"{nameof(this.DeleteJump)} called");
        try
        {
            this.LogbookService.DeleteJump(new LoggedJump());
            return await Task.FromResult(this.Ok());
        }
        catch (Exception ex)
        {
            this.Logger.LogError(ex, "Failed to delete jump");
            return this.StatusCode(500, "Unkown error. Failed to delete jump");
        }
        finally
        {
            this.Logger.LogInformation($"{nameof(this.DeleteJump)} completed"); 
        }
    }
}
