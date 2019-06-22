namespace EEFApps.ApiInstructions.DataInstructions.Instructions
{
    using System.Net;
    using System.Threading.Tasks;
    using EEFApps.ApiInstructions.BaseEntities.Entities.Interfaces;
    using EEFApps.ApiInstructions.DataInstructions.Exceptions;
    using EEFApps.ApiInstructions.DataInstructions.Instructions.Structures;
    using Microsoft.EntityFrameworkCore;

    public class CreationParentRelatedUserContextedInstruction<TEntity, TParentEntity, TParentId, TUserId> : CreationInstruction<TEntity>
        where TEntity : class, new()
        where TParentEntity : class, IEntityWithId<TParentId>, IEntityWithUserContext<TUserId>, new()
    {
        private readonly DbContext context;
        private readonly TEntity entity = null;
        private readonly TParentId parentEntityId;
        private readonly TUserId userId;

        public CreationParentRelatedUserContextedInstruction(DbContext context, TEntity entity, TParentId parentEntityId, TUserId userId) : base(context, entity)
        {
            this.context = context;
            this.entity = entity;
            this.parentEntityId = parentEntityId;
            this.userId = userId;
        }

        public override async Task<TEntity> Execute()
        {
            // If parent is not set it means that creating entity is top level record
            if (this.parentEntityId != null)
            {
                try
                {
                    await new ReceivingUserContextedInstruction<TParentEntity, TParentId, TUserId>(
                        this.context,
                        new ReceivingInstructionParams<TParentEntity, TParentId> { Id = this.parentEntityId },
                        this.userId).Execute();
                }
                catch (InstructionException ex)
                {
                    if (ex.Message.Contains("The resource wasn't found!") && ex.httpStatusCode == HttpStatusCode.NotFound)
                    {
                        throw new InstructionException("The parent entity wasn't found!", HttpStatusCode.NotFound);
                    }

                    throw;
                }
            }

            return await base.Execute();
        }
    }
}
