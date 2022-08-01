using Microsoft.AspNetCore.Mvc.Rendering;

namespace ParkyWeb.Models.ViewModels;

public class TrailUpsertViewModel
{
    public IEnumerable<SelectListItem>? NationalParkList { get; set; }
    public TrailViewModel Trail { get; set; }
}