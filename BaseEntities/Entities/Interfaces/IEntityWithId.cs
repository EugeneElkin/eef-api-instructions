namespace BaseEntities.Entities
{
    public interface IEntityWithId<T>
    {
        T Id { get; set; }
    }
}
