namespace EEFApps.ApiInstructions.DataInstructions.Instructions
{
    using System.Net;
    using EEFApps.ApiInstructions.BaseEntities.Entities;
    using EEFApps.ApiInstructions.DataInstructions.Exceptions;
    using EEFApps.ApiInstructions.DataInstructions.Instructions.Structures;
    using Microsoft.EntityFrameworkCore;

    public class ReceivingUserConextedInstruction<TEntity, TId> : ReceivingInstruction<TEntity, TId>
        where TEntity : BaseEntityWithUserContext<TId>, new()
    {
        public ReceivingUserConextedInstruction(DbContext context, ReceivingInstructionParams<TId> options, string userId)
        : base(context, options, x => x.Id.Equals(options.Id) && x.UserId.Equals(userId))
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new InstructionException("User ID must be provided for the instruction!", HttpStatusCode.BadRequest);
            }
        }
    }
}
