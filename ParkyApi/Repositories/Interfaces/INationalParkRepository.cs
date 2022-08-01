using ParkyApi.Models;
using System.Linq.Expressions;

namespace ParkyApi.Repositories.Interfaces;

public interface INationalParkRepository
{
    Task<ICollection<NationalPark>> GetAllAsync();
    Task<NationalPark?> GetByIdAsync(int id);
    Task<bool> NationalParkExistsAsync(Expression<Func<NationalPark, bool>> expression);
    Task<NationalPark> CreateAsync(NationalPark nationalPark);
    Task<NationalPark> UpdateAsync(NationalPark nationalPark);
}