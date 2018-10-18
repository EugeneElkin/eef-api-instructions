using EEFApps.ApiInstructions.BaseEntities.Entities.Interfaces;

namespace EEFApps.ApiInstructions.BaseEntities.Entities
{
    public abstract class BaseEntityWithParentUserContexted<TId, TUserId> : BaseEntity<TId>, IEntityWithUserContext<TUserId>, IEntityWithParent<TId>
    {
        public TUserId UserId { get; set; }
        public TId ParentId { get; set; }
    }
}
