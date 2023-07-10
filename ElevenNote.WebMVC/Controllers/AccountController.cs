using ElevenNote.Models.User;
using ElevenNote.Services.User;
using Microsoft.AspNetCore.Mvc;

namespace ElevenNote.WebMVC.Controllers;

public class AccountController : Controller //initial setup doorway
{
    private readonly IUserService _userService; //class level variable, for use in multiple methods; if the
    // source is from outside the class, logic started before i start using my class 
    public AccountController(IUserService userService) //class lifecycle
    {
        _userService = userService;
    }

    // GET Action for Register -> Returns the view to the user
    public IActionResult Register()
    {
        return View();
    }

    // POST Action for Register -> When the user submits their data from the view
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(UserRegister model)
    {
        // first validate the request model, reject if invalid
        if (!ModelState.IsValid)
        {
            return View(model);
        }
        // Try to register the user, reject if faild
        var registerResult = await _userService.RegisterUserAsync(model);
        if (registerResult == false)
        {
            //TODO: add error to page
            return View(model);
        }

        // Login the new user, redirect to home after
        UserLogin loginModel = new()
        {
            UserName = model.Username,
            Password = model.Password
        };
        await _userService.LoginAsync(loginModel);
        return RedirectToAction("Index", "Home");
    }

    // GET login
public IActionResult Login()
{
    return View();
}
    // Post login
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(UserLogin model)
    {
        var loginResult= await _userService.LoginAsync(model);
        if (loginResult == false)
        {
            return View(model);
        }

        return RedirectToAction("Index", "Home");
    }

    public async Task<IActionResult> Logout()
    {
        await _userService.LogoutAsync();
        return RedirectToAction("Index", "Home");
    }
}