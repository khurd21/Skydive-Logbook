using Microsoft.AspNetCore.Mvc;

namespace Logbook.APIs;

public interface ILogbookAPI
{
    Task<IActionResult> ListJumps();
    Task<IActionResult> LogJump();
    Task<IActionResult> EditJump();
    Task<IActionResult> DeleteJump();
}