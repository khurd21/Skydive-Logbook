using Microsoft.AspNetCore.Mvc;
using Logbook.Requests.Logbook;

namespace Logbook.APIs;

public interface ILogbookAPI
{
    Task<IActionResult> ListJumps(ListJumpsRequest request);
    Task<IActionResult> LogJump(LogJumpRequest request);
    Task<IActionResult> EditJump(EditJumpRequest request);
    Task<IActionResult> DeleteJump(DeleteJumpRequest request);
}