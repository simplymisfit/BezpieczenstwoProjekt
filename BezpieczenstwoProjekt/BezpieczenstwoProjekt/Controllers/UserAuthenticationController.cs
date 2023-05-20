using BezpieczenstwoProjekt.Models.Dto;
using BezpieczenstwoProjekt.Repositories.Implementation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BezpieczenstwoProjekt.Controllers;

public class UserAuthenticationController : Controller
{
    private readonly UserAuthenticationService _service;
    
    public UserAuthenticationController(UserAuthenticationService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Registration(Registration model)
    {
        if (!ModelState.IsValid) return View(model);

        model.Role = "user";
        
        var result = await _service.RegisterAsync(model);
        TempData["msg"] = result.StatusMessage;
        return RedirectToAction(nameof(Registration));
    }
    
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(Login model)
    {
        if (!ModelState.IsValid) return View(model);

        var result = await _service.LoginAsync(model);
        if (result.StatusCode == 1)
        {
            return RedirectToAction("Display", "Dashboard");
        }
        else
        {
            TempData["msg"] = result.StatusMessage;
            return RedirectToAction(nameof(Login));
        }
    }

    [Authorize]
    public async Task Logout()
    {
        await _service.LogoutAsync();
    }

    public async Task<IActionResult> Reg()
    {
        var model = new Registration
        {
            Username = "admin",
            Name = "Filip",
            Email = "admin@gmail.com",
            Password = "!QAZ2wsx",
            Role = "admin"
        };
        
        var result = await _service.RegisterAsync(model);
        return Ok(result);
    }
}