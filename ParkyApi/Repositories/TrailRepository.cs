using ParkyApi.Models;
using ParkyApi.Repositories.Interfaces;
using System.Linq.Expressions;

namespace ParkyApi.Repositories;

public class TrailRepository : ITrailRepository
{
    private readonly IParkyContext _parkyRepository;

    public TrailRepository(IParkyContext parkyRepository)
    {
        _parkyRepository = parkyRepository;
    }

    public async Task<Trail?> GetByIdAsync(int id)
    {
        return await _parkyRepository.GetByIdAsync<Trail>(id, x => x.NationalPark);
    }

    public async Task<ICollection<Trail>> GetTrailsInNationalParkAsync(int nationalParkId)
    {
        return await _parkyRepository.WhereAsync<Trail>(true, x => x.NationalPark.Id == nationalParkId, includes: x => x.NationalPark);
    }

    public async Task<ICollection<Trail>> GetAllAsync()
    {
        return await _parkyRepository.WhereAsync<Trail>(true, x => true, x => x.NationalPark);
    }

    public async Task<bool> TrailExistsAsync(Expression<Func<Trail, bool>> expression)
    {
        return await _parkyRepository.AnyAsync(expression);
    }

    public async Task<Trail> CreateAsync(Trail trail)
    {
        await _parkyRepository.CreateAsync(trail);

        return trail;
    }

    public async Task<Trail> UpdateAsync(Trail trail)
    {
        await _parkyRepository.UpdateAsync(trail);

        return trail;
    }
}