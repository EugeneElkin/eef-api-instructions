namespace EEFApps.ApiInstructions.BaseEntities.Entities
{
    using EEFApps.ApiInstructions.BaseEntities.Entities.Interfaces;

    public class BaseEntityWithUserContext<T> : BaseEntity<T>, IEntityWithUserContext
    {
        public string UserId { get; set; }
    }
}
