using ParkyWeb.Models.ViewModels;

namespace ParkyWeb.Services.Interfaces;

public interface ITrailService
{
    Task<TrailViewModel> GetAsync(int id);
    Task<IEnumerable<TrailViewModel>> GetAllAsync();
    Task<bool> CreateAsync(TrailViewModel trailCreateDto);
    Task<bool> UpdateAsync(TrailViewModel trailUpdateDto, int id);
    Task<bool> DeleteAsync(int id);
}