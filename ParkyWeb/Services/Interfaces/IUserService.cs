using ParkyWeb.Models.ViewModels;

namespace ParkyWeb.Services.Interfaces;

public interface IUserService
{
    Task<bool> CreateAsync(UserViewModel userViewModel);
    Task<UserViewModel> AuthenticateAsync(UserViewModel userViewModel);
}