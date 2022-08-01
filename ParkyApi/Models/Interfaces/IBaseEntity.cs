namespace ParkyApi.Models
{
    public interface IBaseEntity
    {
        int Id { get; }
        bool Deleted { get; }
        DateTime Created { get; }
        DateTime? LastUpdated { get; }
        DateTime? DeletedDate { get; }
    }
}
