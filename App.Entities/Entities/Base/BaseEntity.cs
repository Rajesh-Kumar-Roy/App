namespace App.Entities.Entities.Base
{
    public interface IBaseEntity
    {
        long Id { get; set; }
    }

    public interface IAudit
    {
        DateTime? CreatedAt { get; set; }
        long? CreatedBy { get; set; }
        DateTime? UpdatedAt { get; set; }
        long? UpdatedBy { get; set; }
        bool IsDelete { get; set; }
    }

    public abstract class BaseEntity : IBaseEntity, IAudit
    {
        public long Id { get; set; }
        public DateTime? CreatedAt { get; set; }
        public long? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public long? UpdatedBy { get; set; }
        public bool IsDelete { get; set; }
    }

}
