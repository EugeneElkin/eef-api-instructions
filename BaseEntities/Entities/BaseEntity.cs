namespace ApiInstructions.BaseEntities.Entities
{
    using ApiInstructions.BaseEntities.Entities.Interfaces;
    using System.ComponentModel.DataAnnotations;

    public abstract class BaseEntity<T> : IEntityWithId<T>, IVersionedEntity
    {
        [Key]
        public T Id { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}
