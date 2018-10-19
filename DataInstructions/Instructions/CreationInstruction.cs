namespace EEFApps.ApiInstructions.DataInstructions.Instructions
{
    using System.Net;
    using System.Threading.Tasks;
    using EEFApps.ApiInstructions.DataInstructions.Exceptions;
    using EEFApps.ApiInstructions.DataInstructions.Instructions.Interfaces;
    using Microsoft.EntityFrameworkCore;

    public class CreationInstruction<TEntity> : IOperationInstruction<TEntity>
        where TEntity : class, new()
    {
        private readonly DbContext context;
        private readonly TEntity entity = null;

        public CreationInstruction(DbContext context, TEntity entity)
        {
            this.context = context;
            this.entity = entity;
        }

        public virtual async Task<TEntity> Execute()
        {
            try
            {
                this.context.Add<TEntity>(this.entity);
                await this.context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException != null && ex.InnerException.Message.Contains("Cannot insert the value NULL into column"))
                {
                    var messageParts = ex.InnerException.Message.Split("'");
                    var targetField = messageParts != null && messageParts.Length > 1 ? messageParts[1] : null;

                    if (targetField == null)
                    {
                        throw;
                    }

                    throw new InstructionException("The property [{0}] must be provided!", HttpStatusCode.BadRequest, targetField);
                }
            }

            return this.entity;
        }
    }
}
