using ParkyApi.Models;
using System.Linq.Expressions;

namespace ParkyApi.Repositories.Interfaces;

public interface IUserRepository
{
    Task<bool> UserExistsAsync(Expression<Func<User, bool>> expression);
    Task<User?> AuthenticateAsync(string username, string password);
    Task<User> CreateAsync(User user);
}