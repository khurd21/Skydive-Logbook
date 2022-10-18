using Logbook.APIs;
using Logbook.Requests.Authentication;
using Logbook.Responses.Authentication;
using LogbookService.Dependencies.AuthenticationService;
using LogbookService.Exceptions;
using LogbookService.Records;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Logbook.Controllers;

[ApiController]
[Authorize]
[Route("account")]
public sealed class AccountController : ControllerBase, IAccountAPI
{
    private ILogger<AccountController> Logger { get; init; }

    private IAuthenticationService AuthenticationService { get; init; }

    public AccountController(ILogger<AccountController> logger, IAuthenticationService authenticationService)
    {
        this.Logger = logger;
        this.AuthenticationService = authenticationService;
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        this.Logger.LogInformation($"{nameof(this.Register)} called");
        try
        {
            SkydiverInfo newSkydiver = this.AuthenticationService.Register(
                new()
                {
                    Email = request.Email,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    USPAMembershipNumber = request.USPAMembershipNumber,
                    USPALicenseNumber = request.USPALicenseNumber,
                }, request.Password!);
            return await Task.FromResult(
                this.Ok(
                    new RegisterResponse() { SkydiverInfo = newSkydiver }));
        }
        catch (SkydiverAlreadyExistsException ex)
        {
            this.Logger.LogWarning(ex, $"{nameof(this.Register)} failed");
            return await Task.FromResult(
                this.Problem(
                    detail: $"Skydiver with USPA membership number {request.USPAMembershipNumber} already exists",
                    statusCode: StatusCodes.Status409Conflict));
        }
        catch (Exception ex)
        {
            this.Logger.LogWarning(ex, "Failed to register");
            return await Task.FromResult(
                this.Problem(
                    detail: "Failed to register",
                    statusCode: StatusCodes.Status500InternalServerError));
        }
        finally
        {
            this.Logger.LogInformation($"{nameof(this.Register)} finished");
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] AuthenticateRequest request)
    {
        this.Logger.LogInformation($"{nameof(this.Login)} called");
        try
        {
            this.AuthenticationService.Login(request.Email!, request.Password!);
            return await Task.FromResult(this.Ok());
        }
        catch (Exception ex)
        {
            this.Logger.LogError(ex, $"{nameof(this.Login)} failed");
            return await Task.FromResult(
                this.Problem(
                    detail: "Failed to login",
                    statusCode: StatusCodes.Status500InternalServerError));
        }
        finally
        {
            this.Logger.LogInformation($"{nameof(this.Login)} finished");
        }
    }

    [HttpDelete("delete")]
    public async Task<IActionResult> Delete()
    {
        this.Logger.LogInformation($"{nameof(this.Delete)} called");
        return await Task.FromResult(this.Ok());
    }
}