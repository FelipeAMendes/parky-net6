using ParkyApi.Models;
using ParkyApi.Repositories.Interfaces;
using System.Linq.Expressions;

namespace ParkyApi.Repositories;

public class NationalParkRepository : INationalParkRepository
{
    private readonly IParkyContext _parkyRepository;

    public NationalParkRepository(IParkyContext parkyRepository)
    {
        _parkyRepository = parkyRepository;
    }

    public async Task<NationalPark?> GetByIdAsync(int id)
    {
        return await _parkyRepository.GetByIdAsync<NationalPark>(id);
    }

    public async Task<ICollection<NationalPark>> GetAllAsync()
    {
        return await _parkyRepository.AllAsync<NationalPark>();
    }

    public async Task<bool> NationalParkExistsAsync(Expression<Func<NationalPark, bool>> expression)
    {
        return await _parkyRepository.AnyAsync(expression);
    }

    public async Task<NationalPark> CreateAsync(NationalPark nationalPark)
    {
        await _parkyRepository.CreateAsync(nationalPark);

        return nationalPark;
    }

    public async Task<NationalPark> UpdateAsync(NationalPark nationalPark)
    {
        await _parkyRepository.UpdateAsync(nationalPark);

        return nationalPark;
    }
}