using ParkyWeb.Models.ViewModels;
using Refit;

namespace ParkyWeb.Repositories.Interfaces;

public interface INationalParkRepository
{
    [Get("/api/v1/nationalparks/{id}")]
    Task<NationalParkViewModel> GetAsync(int id, [Authorize] string token);
        
    [Get("/api/v1/nationalparks")]
    Task<IEnumerable<NationalParkViewModel>> GetAllAsync();

    [Post("/api/v1/nationalparks")]
    Task<IApiResponse> CreateAsync([Body] NationalParkViewModel nationalParkDto, [Authorize] string token);

    [Patch("/api/v1/nationalparks/{id}")]
    Task<IApiResponse> UpdateAsync([Body] NationalParkViewModel nationalParkDto, int id, [Authorize] string token);

    [Delete("/api/v1/nationalparks/{id}")]
    Task<IApiResponse> DeleteAsync(int id, [Authorize] string token);
}