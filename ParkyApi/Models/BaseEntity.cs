namespace ParkyApi.Models
{
    public abstract class BaseEntity : IBaseEntity
    {
        public int Id { get; private set; }
        public bool Deleted { get; private set; }
        public DateTime Created { get; private set; }
        public DateTime? LastUpdated { get; private set; }
        public DateTime? DeletedDate { get; private set; }

        public void HasCreated()
        {
            Created = DateTime.Now;
        }

        public void HasUpdated()
        {
            LastUpdated = DateTime.Now;
        }

        public void Delete()
        {
            Deleted = true;
            DeletedDate = DateTime.Now;
        }
    }
}
