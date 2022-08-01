using Microsoft.AspNetCore.Mvc.Rendering;
using ParkyWeb.Models.ViewModels;
using ParkyWeb.Services.Interfaces;

namespace ParkyWeb.Controllers.Extensions;

public static class TrailsControllerExtensions
{
    public static async Task<bool> InsertOrUpdateTrailAsync(this TrailsController _,
                                                            ITrailService trailService,
                                                            TrailViewModel trailViewModel)
    {
        if (trailViewModel.Id != 0)
        {
            var resultUpdate = await trailService.UpdateAsync(trailViewModel, trailViewModel.Id);
            
            return resultUpdate;
        }

        var resultCreate = await trailService.CreateAsync(trailViewModel);

        return resultCreate;
    }

    public static TrailUpsertViewModel GetDefaultViewModel(this TrailsController _,
                                                           IEnumerable<NationalParkViewModel> nationalParkListViewModel,
                                                           TrailViewModel trailViewModel)
    {
        var trailUpsertViewModel = new TrailUpsertViewModel
        {
            Trail = trailViewModel,
            NationalParkList = nationalParkListViewModel.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            })
        };

        return trailUpsertViewModel;
    }
}