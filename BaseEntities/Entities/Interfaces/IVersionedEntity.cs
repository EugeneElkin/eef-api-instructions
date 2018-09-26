namespace EEFApps.ApiInstructions.BaseEntities.Entities.Interfaces
{
    public interface IVersionedEntity
    {
        byte[] RowVersion { get; set; }
    }
}
