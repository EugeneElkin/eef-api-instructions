namespace EEFApps.ApiInstructions.BaseEntities.Entities.Interfaces
{
    public interface IEntityWithId<T>
    {
        T Id { get; set; }
    }
}
