using ParkyApi.Models;
using ParkyApi.Repositories.Extensions;
using ParkyApi.Repositories.Interfaces;
using System.Linq.Expressions;

namespace ParkyApi.Repositories;

public class UserRepository : IUserRepository
{
    private readonly IParkyContext _parkyRepository;

    public UserRepository(IParkyContext parkyRepository)
    {
        _parkyRepository = parkyRepository;
    }

    public async Task<bool> UserExistsAsync(Expression<Func<User, bool>> expression)
    {
        return await _parkyRepository.AnyAsync(expression);
    }

    public async Task<User?> AuthenticateAsync(string username, string password)
    {
        var user = await _parkyRepository.FirstOrDefaultAsync<User>(true, x => x.Username == username && x.Password == password);
        if (user is null)
            return user;

        var token = this.GetToken(user);
        user.DefineToken(token);

        return user;
    }

    public async Task<User> CreateAsync(User user)
    {
        await _parkyRepository.CreateAsync(user);

        return user;
    }
}