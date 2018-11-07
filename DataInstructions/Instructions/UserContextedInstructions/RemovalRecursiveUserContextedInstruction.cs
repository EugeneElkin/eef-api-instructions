namespace EEFApps.ApiInstructions.DataInstructions.Instructions
{
    using EEFApps.ApiInstructions.BaseEntities.Entities.Interfaces;
    using EEFApps.ApiInstructions.DataInstructions.Exceptions;
    using EEFApps.ApiInstructions.DataInstructions.Instructions.Structures;
    using Microsoft.EntityFrameworkCore;
    using System.Net;

    public class RemovalRecursiveUserContextedInstruction<TEntity, TId, TUserId> : RemovalRecursiveInstruction<TEntity, TId>
        where TEntity : class, IEntityWithId<TId>, IEntityWithUserContext<TUserId>, IEntityWithParent<TId>, new()
    {
        public RemovalRecursiveUserContextedInstruction(DbContext context, RemovalInstructionParams<TId> options, TUserId userId) :
            base(context, options, x => x.Id.Equals(options.Id) && x.UserId.Equals(userId))
        {
            if (userId == null)
            {
                throw new InstructionException("User ID must be provided for the instruction!", HttpStatusCode.BadRequest);
            }
        }
    }
}
