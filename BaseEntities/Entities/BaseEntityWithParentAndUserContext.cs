namespace EEFApps.ApiInstructions.BaseEntities.Entities
{
    using EEFApps.ApiInstructions.BaseEntities.Entities.Interfaces;

    public abstract class BaseEntityWithParentAndUserContext<TId, TUserId> : BaseEntity<TId>, IEntityWithUserContext<TUserId>, IEntityWithParent<TId>
    {
        public TUserId UserId { get; set; }
        public TId ParentId { get; set; }
    }
}
