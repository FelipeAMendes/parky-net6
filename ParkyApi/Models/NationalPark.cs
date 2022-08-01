namespace ParkyApi.Models;

public class NationalPark : BaseEntity
{
    public string Name { get; private set; }
    public string State { get; private set; }
    public byte[]? Picture { get; private set; }
    public DateTime Established { get; private set; }
    public IReadOnlyCollection<Trail> Trails { get; private set; }

    public void Edit(string name, string state, DateTime established)
    {
        Name = name;
        State = state;
        Established = established;
        HasUpdated();
    }

    public void DefinePicture(byte[]? picture)
    {
        Picture = picture;
    }
}