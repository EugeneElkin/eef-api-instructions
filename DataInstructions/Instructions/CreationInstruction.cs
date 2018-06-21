namespace DataInstructions.Instructions
{
    using System.Threading.Tasks;
    using BaseEntities.Entities;
    using DataInstructions.Instructions.Interfaces;
    using Microsoft.EntityFrameworkCore;

    public class CreationInstruction<TEntity, TId> : IOperationInstruction<TEntity>
        where TEntity : class, IEntityWithId<TId>, new()
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
