namespace EEFApps.ApiInstructions.DataInstructions.Instructions
{
    using System.Net;
    using System.Threading.Tasks;
    using EEFApps.ApiInstructions.BaseEntities.Entities.Interfaces;
    using EEFApps.ApiInstructions.DataInstructions.Exceptions;
    using EEFApps.ApiInstructions.DataInstructions.Instructions.Structures;
    using Microsoft.EntityFrameworkCore;

    public class UpdateUserContextedInstruction<TEntity, TId, TUserId> : UpdateInstruction<TEntity, TId>
        where TEntity : class, IEntityWithId<TId>, IEntityWithUserContext<TUserId>, new()
    {
        private readonly DbContext context;
        private readonly TId id;
        private readonly TUserId userId;

        public UpdateUserContextedInstruction(DbContext context, TId id, TEntity entity, TUserId userId) : base(context, id, entity)
        {
            if (userId == null)
            {
                throw new InstructionException("User ID must be provided for the instruction!", HttpStatusCode.BadRequest);
            }

            this.context = context;
            this.id = id;
            this.userId = userId;
        }

        public override async Task<bool> Execute()
        {
            await new ReceivingUserContextedInstruction<TEntity, TId, TUserId>(
                this.context,
                new ReceivingInstructionParams<TEntity, TId> { Id = this.id },
                this.userId).Execute();

            // If entity doesn't exist there will be appropriate Instruction exception generated.
            // Below we can be sure that entity exists and belongs to provided user.
            return await base.Execute();
        }
    }
}
