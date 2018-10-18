namespace EEFApps.ApiInstructions.BaseEntities.Entities
{
    public abstract class BaseEntityWithParent<TId>: BaseEntity<TId>
    {
        public TId ParentId { get; set; }
    }
}
