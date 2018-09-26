namespace EEFApps.ApiInstructions.DataInstructions.Instructions
{
    using System.Linq;
    using System.Threading.Tasks;
    using EEFApps.ApiInstructions.BaseEntities.Entities.Interfaces;
    using EEFApps.ApiInstructions.DataInstructions.Instructions.Interfaces;
    using Microsoft.EntityFrameworkCore;

    public class UpdateInstruction<TEntity, TId> : IOperationInstruction<bool>
        where TEntity : class, IEntityWithId<TId>, new()
    {
        private readonly DbContext context;
        private readonly TId id;
        private readonly TEntity entity = null;

        public UpdateInstruction(DbContext context, TId id, TEntity entity)
        {
            this.context = context;
            this.entity = entity;
            this.id = id;
        }

        public async Task<bool> Execute()
        {
            context.Entry<TEntity>(this.entity).State = EntityState.Modified;

            try
            {
                await this.context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!this.EntityExists(this.id))
                {
                    return false;
                }
                else
                {
                    throw;
                }
            }

            return true;
        }

        private bool EntityExists(TId id)
        {
            var dbSet = this.context.Set<TEntity>();
            return dbSet.Any(e => e.Id.Equals(id));
        }
    }
}
