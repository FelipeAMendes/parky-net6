using ParkyWeb.Models.ViewModels;

namespace ParkyWeb.Services.Interfaces;

public interface INationalParkService
{
    Task<NationalParkViewModel> GetAsync(int id);
    Task<IEnumerable<NationalParkViewModel>> GetAllAsync();
    Task<bool> CreateAsync(NationalParkViewModel nationalParkDto);
    Task<bool> UpdateAsync(NationalParkViewModel nationalParkDto);
    Task<bool> DeleteAsync(int id);
}