namespace DataInstructions.Instructions.UserContextedInstructions
{
    using System.Net;
    using EEFApps.ApiInstructions.BaseEntities.Entities.Interfaces;
    using EEFApps.ApiInstructions.DataInstructions.Exceptions;
    using EEFApps.ApiInstructions.DataInstructions.Instructions;
    using EEFApps.ApiInstructions.DataInstructions.Instructions.Structures;
    using Microsoft.EntityFrameworkCore;

    public class ReceivingUserContextedListInstruction<TEntity, TUserId> : ReceivingListInstruction<TEntity>
         where TEntity : class, IEntityWithUserContext<TUserId>, new()
    {
        public ReceivingUserContextedListInstruction(DbContext context, ListInstructionParams<TEntity> options, TUserId userId)
        : base(context, options, x => x.UserId.Equals(userId))
        {
            if (userId == null)
            {
                throw new InstructionException("User ID must be provided for the instruction!", HttpStatusCode.BadRequest);
            }
        }
    }
}
