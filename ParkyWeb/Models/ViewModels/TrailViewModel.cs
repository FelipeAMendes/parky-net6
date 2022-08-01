using System.ComponentModel.DataAnnotations;

namespace ParkyWeb.Models.ViewModels;

public class TrailViewModel
{
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public double Distance { get; set; }
    [Required]
    public double Elevation { get; set; }
    [Required]
    public int Difficult { get; set; }
    public int NationalParkId { get; set; }
    public NationalParkViewModel? NationalPark { get; set; }
}