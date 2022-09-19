using Microsoft.AspNetCore.Mvc;

namespace Logbook.APIs;

public interface IAccountAPI
{
    Task<IActionResult> Register();
    Task<IActionResult> Login();
    Task<IActionResult> Logout();
    Task<IActionResult> Delete();
}