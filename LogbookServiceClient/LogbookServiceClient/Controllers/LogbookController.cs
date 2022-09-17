using Logbook.APIs;
using Microsoft.AspNetCore.Mvc;

namespace Logbook.Controllers;


[ApiController]
[Route("logbook")]
public sealed class LogbookController : ControllerBase, ILogbookAPI
{
    private ILogger<LogbookController> Logger { get; init; }

    public LogbookController(ILogger<LogbookController> logger)
    {
        this.Logger = logger;
    }

    [HttpGet("listjumps")]
    public async Task<IActionResult> ListJumps()
    {
        this.Logger.LogInformation($"{nameof(this.ListJumps)} called");
        return await Task.FromResult(this.Ok());
    }

    [HttpPost("logjump")]
    public async Task<IActionResult> LogJump()
    {
        this.Logger.LogInformation($"{nameof(this.LogJump)} called");
        return await Task.FromResult(this.Ok());
    }

    [HttpPut("editjump")]
    public async Task<IActionResult> EditJump()
    {
        this.Logger.LogInformation($"{nameof(this.EditJump)} called");
        return await Task.FromResult(this.Ok());
    }

    [HttpDelete("deletejump")]
    public async Task<IActionResult> DeleteJump()
    {
        this.Logger.LogInformation($"{nameof(this.DeleteJump)} called");
        return await Task.FromResult(this.Ok());
    }
}
