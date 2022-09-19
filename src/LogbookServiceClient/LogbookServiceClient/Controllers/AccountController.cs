using Logbook.APIs;
using Microsoft.AspNetCore.Mvc;

namespace Logbook.Controllers;

[ApiController]
[Route("account")]
public class AccountController : ControllerBase, IAccountAPI
{
    private ILogger<AccountController> Logger { get; init; }

    public AccountController(ILogger<AccountController> logger)
    {
        this.Logger = logger;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register()
    {
        this.Logger.LogInformation($"{nameof(this.Register)} called");
        return await Task.FromResult(this.Ok());
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login()
    {
        this.Logger.LogInformation($"{nameof(this.Login)} called");
        return await Task.FromResult(this.Ok());
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        this.Logger.LogInformation($"{nameof(this.Logout)} called");
        return await Task.FromResult(this.Ok());
    }

    [HttpDelete("delete")]
    public async Task<IActionResult> Delete()
    {
        this.Logger.LogInformation($"{nameof(this.Delete)} called");
        return await Task.FromResult(this.Ok());
    }
}