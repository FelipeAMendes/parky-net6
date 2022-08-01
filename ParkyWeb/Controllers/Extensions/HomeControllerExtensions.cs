using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using ParkyWeb.Models.ViewModels;
using System.Security.Claims;

namespace ParkyWeb.Controllers.Extensions;

public static class HomeControllerExtensions
{
    public static async Task SaveCookieAsync(this HomeController _, UserViewModel userViewModel, HttpContext httpContext)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, userViewModel.Username),
            new(ClaimTypes.Role, userViewModel.Role),
            new(ClaimTypes.Authentication, userViewModel.Token)
        };
        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        await httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties());
    }

    public static async Task Logout(this HomeController _, HttpContext httpContext)
    {
        await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    }
}