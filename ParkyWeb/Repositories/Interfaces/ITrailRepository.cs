using ParkyWeb.Models.ViewModels;
using Refit;

namespace ParkyWeb.Repositories.Interfaces;

public interface ITrailRepository
{
    [Get("/api/v1/trails/{id}")]
    Task<TrailViewModel> GetAsync(int id, [Authorize] string token);
        
    [Get("/api/v1/trails")]
    Task<IEnumerable<TrailViewModel>> GetAllAsync();

    [Post("/api/v1/trails")]
    Task<IApiResponse> CreateAsync([Body] TrailViewModel trailViewModel, [Authorize] string token);

    [Patch("/api/v1/trails/{id}")]
    Task<IApiResponse> UpdateAsync([Body] TrailViewModel trailViewModel, int id, [Authorize] string token);

    [Delete("/api/v1/trails/{id}")]
    Task<IApiResponse> DeleteAsync(int id, [Authorize] string token);
}