using Logbook.Requests.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace Logbook.APIs;

public interface IAccountAPI
{
    Task<IActionResult> Register(RegisterRequest request);
    Task<IActionResult> Login(AuthenticateRequest request);
    Task<IActionResult> Delete();
}