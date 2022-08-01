using ParkyWeb.Models.ViewModels;
using ParkyWeb.Repositories.Interfaces;
using ParkyWeb.Services.Interfaces;
using System.Security.Claims;

namespace ParkyWeb.Services;

public class TrailService : ITrailService
{
    private readonly ITrailRepository _trailRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public TrailService(ITrailRepository trailRepository, IHttpContextAccessor httpContextAccessor)
    {
        _trailRepository = trailRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<TrailViewModel> GetAsync(int id)
    {
        var response = await _trailRepository.GetAsync(id, GetToken());

        return response;
    }

    public async Task<IEnumerable<TrailViewModel>> GetAllAsync()
    {
        var response = await _trailRepository.GetAllAsync();

        return response;
    }

    public async Task<bool> CreateAsync(TrailViewModel trailViewModel)
    {
        var response = await _trailRepository.CreateAsync(trailViewModel, GetToken());

        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateAsync(TrailViewModel trailViewModel, int id)
    {
        var response = await _trailRepository.UpdateAsync(trailViewModel, id, GetToken());

        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var response = await _trailRepository.DeleteAsync(id, GetToken());

        return response.IsSuccessStatusCode;
    }

    private string GetToken()
    {
        var claims = _httpContextAccessor.HttpContext?.User.Claims;
        return claims?.FirstOrDefault(x => x.Type == ClaimTypes.Authentication)?.Value ?? string.Empty;
    }
}