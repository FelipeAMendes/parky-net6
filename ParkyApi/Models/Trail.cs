using ParkyApi.Models.Enums;

namespace ParkyApi.Models;

public class Trail : BaseEntity
{
    public string Name { get; private set; }
    public double Distance { get; private set; }
    public double Elevation { get; set; }
    public DifficultType Difficult { get; private set; }
    public virtual NationalPark NationalPark { get; private set; }

    public void Edit(string name, double distance, double elevation, DifficultType difficult)
    {
        Name = name;
        Distance = distance;
        Elevation = elevation;
        Difficult = difficult;
        HasUpdated();
    }

    public void DefineNationalPark(NationalPark nationalPark)
    {
        NationalPark = nationalPark;
    }
}