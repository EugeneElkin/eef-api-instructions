namespace EEFApps.ApiInstructions.BaseEntities.Entities.Interfaces
{
    public interface IEntityWithParent<T>
    {
        T ParentId { get; set; }
    }
}
