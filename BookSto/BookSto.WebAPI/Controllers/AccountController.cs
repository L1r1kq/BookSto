// BookSto.WebAPI/Controllers/AccountController.cs
using BookSto.Application.Features.Auth.Commands;
using BookSto.Domain.Models;
using BookSto.WebAPI.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BookSto.WebAPI.Controllers;

public class AccountController : Controller
{
    private readonly IMediator _mediator;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public AccountController(IMediator mediator, SignInManager<ApplicationUser> signInManager)
    {
        _mediator = mediator;
        _signInManager = signInManager;
    }

    [HttpGet]
    public IActionResult Register() => View();

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var result = await _mediator.Send(new RegisterCommand 
        { 
            Email = model.Email, 
            Password = model.Password, 
            ConfirmPassword = model.ConfirmPassword, // Добавлено
            FirstName = model.FirstName, 
            LastName = model.LastName, 
            AcceptTerms = model.AcceptTerms // Добавлено
        });

        if (result.IsSuccess)
            return RedirectToAction("Index", "Home");

        ModelState.AddModelError(string.Empty, result.Error);
        return View(model);
    }

    [HttpGet]
    public IActionResult Login() => View();
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var result = await _mediator.Send(new LoginCommand 
        { 
            Email = model.Email, 
            Password = model.Password 
        });

        if (result.IsSuccess)
            return RedirectToAction("Index", "Home");

        ModelState.AddModelError(string.Empty, result.Error);
        return View(model);
    }
    
    
}