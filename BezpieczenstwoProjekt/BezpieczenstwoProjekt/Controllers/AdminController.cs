namespace BezpieczenstwoProjekt.Controllers;

[Authorize(Roles = "admin")]
public class AdminController : Controller
{
    public IActionResult Display()
    {
        return View();
    }
}