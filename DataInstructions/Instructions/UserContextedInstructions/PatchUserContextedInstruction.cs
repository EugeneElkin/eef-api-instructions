namespace EEFApps.ApiInstructions.DataInstructions.Instructions
{
    using System.Net;
    using EEFApps.ApiInstructions.BaseEntities.Entities;
    using EEFApps.ApiInstructions.BaseEntities.Entities.Interfaces;
    using EEFApps.ApiInstructions.DataInstructions.Exceptions;
    using EEFApps.ApiInstructions.DataInstructions.Instructions.Structures;
    using Microsoft.EntityFrameworkCore;

    public class PatchUserContextedInstruction<TEntity, TId, TUserId> : PatchInstruction<TEntity, TId>
        where TEntity : BaseEntity<TId>, IEntityWithUserContext<TUserId>, new()
    {
        public PatchUserContextedInstruction(DbContext context, PatchInstructionParams<TEntity, TId> options, TUserId userId) 
            : base(context, options, x => x.Id.Equals(options.Id) && x.UserId.Equals(userId))
        {
            if (userId == null)
            {
                throw new InstructionException("User ID must be provided for the instruction!", HttpStatusCode.BadRequest);
            }
        }
    }
}
