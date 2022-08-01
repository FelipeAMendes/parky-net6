using ParkyWeb.Models.ViewModels;
using ParkyWeb.Repositories.Interfaces;
using ParkyWeb.Services.Interfaces;

namespace ParkyWeb.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<bool> CreateAsync(UserViewModel userViewModel)
    {
        var response = await _userRepository.CreateAsync(userViewModel);

        return response.IsSuccessStatusCode;
    }

    public async Task<UserViewModel> AuthenticateAsync(UserViewModel userViewModel)
    {
        var response = await _userRepository.AuthenticateAsync(userViewModel);

        return response.IsSuccessStatusCode
            ? response.Content!
            : new UserViewModel();
    }
}