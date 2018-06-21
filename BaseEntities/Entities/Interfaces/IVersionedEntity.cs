namespace BaseEntities.Entities
{
    public interface IVersionedEntity
    {
        byte[] RowVersion { get; set; }
    }
}
