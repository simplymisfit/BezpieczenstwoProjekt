using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BezpieczenstwoProjekt.Controllers;

public class DashboardController : Controller
{
    [Authorize(Roles = "admin")]
    public IActionResult Display()
    {
        return View();
    }
}