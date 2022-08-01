using Microsoft.AspNetCore.Mvc;
using ParkyWeb.Controllers.Extensions;
using ParkyWeb.Models.ViewModels;
using ParkyWeb.Services.Interfaces;

namespace ParkyWeb.Controllers;

public class HomeController : Controller
{
    private readonly INationalParkService _nationalParkService;
    private readonly ITrailService _trailService;
    private readonly IUserService _userService;

    public HomeController(INationalParkService nationalParkService, ITrailService trailService, IUserService userService)
    {
        _nationalParkService = nationalParkService;
        _trailService = trailService;
        _userService = userService;
    }

    public async Task<IActionResult> Index()
    {
        var indexViewModel = new IndexViewModel
        {
            NationalParkList = await _nationalParkService.GetAllAsync(),
            TrailList = await _trailService.GetAllAsync()
        };
        return View(indexViewModel);
    }

    public IActionResult Login()
    {
        var userViewModel = new UserViewModel();
        return View(userViewModel);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(UserViewModel userViewModel)
    {
        var userViewModelResponse = await _userService.AuthenticateAsync(userViewModel);
        if (userViewModelResponse.Token is null)
            return View();

        await this.SaveCookieAsync(userViewModelResponse, HttpContext);

        TempData["alert"] = $"Welcome {userViewModel.Username}!";
        return RedirectToAction("Index");
    }

    public IActionResult Register() => View();

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(UserViewModel userViewModel)
    {
        var registered = await _userService.CreateAsync(userViewModel);

        TempData["alert"] = "Registration successful!";
        return !registered
            ? View(userViewModel)
            : RedirectToAction("Login");
    }

    public async Task<IActionResult> Logout()
    {
        await this.Logout(HttpContext);

        return RedirectToAction("Index");
    }

    public IActionResult AccessDenied() => View();

    public IActionResult Privacy() => View();

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() => View();
}