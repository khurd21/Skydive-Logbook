using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Logbook.Controllers.V1;

[ApiController]
[ApiVersion("1.0")]
[Authorize]
[Route("api/v{version:ApiVersion}/account")]
public sealed class AccountController : ControllerBase
{
    private ILogger<AccountController> Logger { get; init; }

    public AccountController(ILogger<AccountController> logger)
    {
        this.Logger = logger;
    }

    [HttpGet("user_info")]
    public async Task<IActionResult> UserInfo()
    {
        this.Logger.LogInformation($"{nameof(this.UserInfo)} called");
        try
        {
            // Get the user id for the current user to retrieve user info from the database
            string userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            this.Logger.LogInformation($"User id: {userId}");
            return await Task.FromResult(this.Ok());
        }
        catch (Exception ex)
        {
            this.Logger.LogWarning(ex, "Failed to get user info");
            return await Task.FromResult(
                this.Problem(
                    detail: "Failed to get user info",
                    statusCode: StatusCodes.Status500InternalServerError));
        }
    }
}
