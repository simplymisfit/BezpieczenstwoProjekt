using BezpieczenstwoProjekt.Models.Dto;
using BezpieczenstwoProjekt.Repositories.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BezpieczenstwoProjekt.Controllers;

public class UserAuthenticationController : Controller
{
    private readonly IUserAuthenticationService _authService;
    public UserAuthenticationController(IUserAuthenticationService authService)
    {
        this._authService = authService;
    }


    public IActionResult Login()
    {
        return View();
    }


    [HttpPost]
    public async Task<IActionResult> Login(Login model)
    {
        if (!ModelState.IsValid)
            return View(model);
        var result = await _authService.LoginAsync(model);
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

    public IActionResult Registration()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Registration(Registration model)
    {
        if (!ModelState.IsValid) { return View(model); }
        model.Role = "user";
        var result = await this._authService.RegisterAsync(model);
        TempData["msg"] = result.StatusMessage;
        return RedirectToAction(nameof(Registration));
    }

    [Authorize]
    public async Task<IActionResult> Logout()
    {
        await this._authService.LogoutAsync();
        return RedirectToAction(nameof(Login));
    }

    [AllowAnonymous]
    public async Task<IActionResult> RegisterAdmin()
    {
        Registration model = new Registration
        {
            Username = "admin",
            Email = "admin@gmail.com",
            FirstName = "John",
            LastName = "Doe",
            Password = "Admin@12345#"
        };
        model.Role = "admin";
        var result = await this._authService.RegisterAsync(model);
        return Ok(result);
    }

    [Authorize]
    public IActionResult ChangePassword()
    {
        return View();
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> ChangePassword(ChangePassword model)
    {
        if (!ModelState.IsValid)
            return View(model);
        var result = await _authService.ChangePasswordAsync(model, User.Identity.Name);
        TempData["msg"] = result.StatusMessage;
        return RedirectToAction(nameof(ChangePassword));
    }
}