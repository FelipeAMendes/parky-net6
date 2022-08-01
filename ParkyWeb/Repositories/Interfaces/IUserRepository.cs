using ParkyWeb.Models.ViewModels;
using Refit;

namespace ParkyWeb.Repositories.Interfaces;

public interface IUserRepository
{
    [Post("/api/v1/users")]
    Task<IApiResponse> CreateAsync([Body] UserViewModel userViewModel);

    [Post("/api/v1/users/authenticate")]
    Task<IApiResponse<UserViewModel>> AuthenticateAsync([Body] UserViewModel userViewModel);
}