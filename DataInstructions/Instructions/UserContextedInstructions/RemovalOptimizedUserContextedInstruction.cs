namespace EEFApps.ApiInstructions.DataInstructions.Instructions
{
    using System.Net;
    using EEFApps.ApiInstructions.BaseEntities.Entities.Interfaces;
    using EEFApps.ApiInstructions.DataInstructions.Exceptions;
    using EEFApps.ApiInstructions.DataInstructions.Instructions.Structures;
    using Microsoft.EntityFrameworkCore;

    public class RemovalOptimizedUserContextedInstruction<TEntity, TId, TUserId> : RemovalOptimizedInstruction<TEntity, TId>
        where TEntity : class, IEntityWithId<TId>, IVersionedEntity, IEntityWithUserContext<TUserId>, new()
    {
        public RemovalOptimizedUserContextedInstruction(DbContext context, RemovalInstructionParams<TId> options, TUserId userId) 
            : base(context, options, new TEntity() { Id = options.Id, RowVersion = options.RowVersion, UserId = userId })
        {
            if (userId == null)
            {
                throw new InstructionException("User ID must be provided for the instruction!", HttpStatusCode.BadRequest);
            }
        }
    }
}
