using Microsoft.AspNetCore.Mvc;

namespace BezpieczenstwoProjekt.Controllers;

public class UserAuthenticationController : Controller
{
    public IActionResult Login()
    {
        return View();
    }
}