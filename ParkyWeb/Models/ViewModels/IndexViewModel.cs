namespace ParkyWeb.Models.ViewModels;

public class IndexViewModel
{
    public IEnumerable<NationalParkViewModel> NationalParkList { get; set; }
    public IEnumerable<TrailViewModel> TrailList { get; set; }
}