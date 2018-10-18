namespace EEFApps.ApiInstructions.BaseEntities.Entities
{
    using EEFApps.ApiInstructions.BaseEntities.Entities.Interfaces;

    public abstract class BaseEntityWithUserContext<TId, TUserId> : BaseEntity<TId>, IEntityWithUserContext<TUserId>
    {
        public TUserId UserId { get; set; }
    }
}
