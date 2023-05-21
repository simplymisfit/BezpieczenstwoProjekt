﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace BezpieczenstwoProjekt.Controllers;

[Authorize(Roles = "admin")]
public class AdminController : Controller
{
    public IActionResult Display()
    {
        return View();
    }
}