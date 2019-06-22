namespace EEFApps.ApiInstructions.DataInstructions.Instructions
{
    using System.Net;
    using EEFApps.ApiInstructions.BaseEntities.Entities.Interfaces;
    using EEFApps.ApiInstructions.DataInstructions.Exceptions;
    using EEFApps.ApiInstructions.DataInstructions.Instructions.Structures;
    using Microsoft.EntityFrameworkCore;

    public class ReceivingUserContextedInstruction<TEntity, TId, TUserId> : ReceivingInstruction<TEntity, TId>
        where TEntity : class, IEntityWithId<TId>, IEntityWithUserContext<TUserId>, new()
    {
        public ReceivingUserContextedInstruction(DbContext context, ReceivingInstructionParams<TEntity, TId> options, TUserId userId)
        : base(context, options, x => x.Id.Equals(options.Id) && x.UserId.Equals(userId))
        {
            if (userId == null)
            {
                throw new InstructionException("User ID must be provided for the instruction!", HttpStatusCode.BadRequest);
            }
        }
    }
}
