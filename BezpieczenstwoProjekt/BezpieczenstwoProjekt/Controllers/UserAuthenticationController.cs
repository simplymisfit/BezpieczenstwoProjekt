using BezpieczenstwoProjekt.Models.Dto;
using BezpieczenstwoProjekt.Repositories.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BezpieczenstwoProjekt.Controllers;

public class UserAuthenticationController : Controller
{
    private readonly IUserAuthenticationService _service;
    
    public UserAuthenticationController(IUserAuthenticationService service)
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
            FirstName = "Filip",
            LastName = "Nazwisko",
            Email = "admin@gmail.com",
            Password = "!QAZ2wsx",
            Role = "admin"
        };
        
        var result = await _service.RegisterAsync(model);
        return Ok(result);
    }
    
    [Authorize]
    [HttpPost]
    public async Task<IActionResult>ChangePassword(ChangePassword model)
    {
        if (!ModelState.IsValid)
            return View(model);
        var result = await _service.ChangePasswordAsync(model, User.Identity.Name);
        TempData["msg"] = result.StatusMessage;
        return RedirectToAction(nameof(ChangePassword));
    }
}