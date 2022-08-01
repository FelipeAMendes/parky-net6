using ParkyApi.Models;
using System.Linq.Expressions;

namespace ParkyApi.Repositories.Interfaces;

public interface ITrailRepository
{
    Task<ICollection<Trail>> GetAllAsync();
    Task<ICollection<Trail>> GetTrailsInNationalParkAsync(int nationalParkId);
    Task<Trail?> GetByIdAsync(int id);
    Task<bool> TrailExistsAsync(Expression<Func<Trail, bool>> expression);
    Task<Trail> CreateAsync(Trail trail);
    Task<Trail> UpdateAsync(Trail trail);
}