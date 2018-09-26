namespace EEFApps.ApiInstructions.DataInstructions.Instructions
{
    using System.Threading.Tasks;
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

        public async Task<TEntity> Execute()
        {
            this.context.Add<TEntity>(this.entity);
            await this.context.SaveChangesAsync();

            return this.entity;
        }
    }
}
