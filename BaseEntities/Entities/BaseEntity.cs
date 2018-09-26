namespace EEFApps.ApiInstructions.BaseEntities.Entities
{
    using System.ComponentModel.DataAnnotations;
    using EEFApps.ApiInstructions.BaseEntities.Entities.Interfaces;

    public abstract class BaseEntity<T> : IEntityWithId<T>, IVersionedEntity
    {
        [Key]
        public T Id { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}
