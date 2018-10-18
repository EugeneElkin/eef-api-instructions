namespace EEFApps.ApiInstructions.BaseEntities.Entities
{
    using System.ComponentModel.DataAnnotations;
    using EEFApps.ApiInstructions.BaseEntities.Entities.Interfaces;

    public abstract class BaseEntity<TId> : IEntityWithId<TId>, IVersionedEntity
    {
        [Key]
        public TId Id { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}
