namespace EEFApps.ApiInstructions.DataInstructions.Instructions
{
    using EEFApps.ApiInstructions.BaseEntities.Entities;
    using EEFApps.ApiInstructions.DataInstructions.Exceptions;
    using EEFApps.ApiInstructions.DataInstructions.Instructions.Structures;
    using Microsoft.EntityFrameworkCore;

    public class PatchUserContextedInstruction<TEntity, TId> : PatchInstruction<TEntity, TId>
        where TEntity : BaseEntityWithUserContext<TId>, new()
    {
        public PatchUserContextedInstruction(DbContext context, PatchInstructionParams<TEntity, TId> options) 
            : base(context, options, x => x.Id.Equals(options.id) && x.UserId.Equals(options.userId))
        {
            if (string.IsNullOrWhiteSpace(options.userId))
            {
                throw new InstructionException("User ID must be provided for the instruction!");
            }
        }
    }
}
