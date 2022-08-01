using System.Security.Claims;
using ParkyWeb.Models.ViewModels;
using ParkyWeb.Repositories.Interfaces;
using ParkyWeb.Services.Interfaces;

namespace ParkyWeb.Services;

public class NationalParkService : INationalParkService
{
    private readonly INationalParkRepository _nationalParkRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public NationalParkService(INationalParkRepository nationalParkRepository, IHttpContextAccessor httpContextAccessor)
    {
        _nationalParkRepository = nationalParkRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<NationalParkViewModel> GetAsync(int id)
    {
        var response = await _nationalParkRepository.GetAsync(id, GetToken());
        
        return response;
    }

    public async Task<IEnumerable<NationalParkViewModel>> GetAllAsync()
    {
        var response = await _nationalParkRepository.GetAllAsync();

        return response;
    }

    public async Task<bool> CreateAsync(NationalParkViewModel nationalParkViewModel)
    {
        var response = await _nationalParkRepository.CreateAsync(nationalParkViewModel, GetToken());

        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateAsync(NationalParkViewModel nationalParkViewModel)
    {
        var response = await _nationalParkRepository.UpdateAsync(nationalParkViewModel, nationalParkViewModel.Id, GetToken());

        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var response = await _nationalParkRepository.DeleteAsync(id, GetToken());

        return response.IsSuccessStatusCode;
    }

    private string GetToken()
    {
        var claims = _httpContextAccessor.HttpContext?.User.Claims;
        return claims?.FirstOrDefault(x => x.Type == ClaimTypes.Authentication)?.Value ?? string.Empty;
    }
}