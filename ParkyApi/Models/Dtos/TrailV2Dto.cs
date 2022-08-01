using ParkyApi.Models.Enums;

namespace ParkyApi.Models.Dtos;

public class TrailV2Dto
{
    public int Id { get; set; }
    public string TrailName { get; set; }
    public double TrailDistance { get; set; }
    public DifficultType TrailDifficult { get; set; }
    public string NationalParkName { get; set; }
    public string NationalParkState { get; set; }
}