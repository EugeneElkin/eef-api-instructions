namespace EEFApps.ApiInstructions.DataInstructions.Instructions
{
    using System.Linq;
    using System.Threading.Tasks;
    using EEFApps.ApiInstructions.BaseEntities.Entities.Interfaces;
    using EEFApps.ApiInstructions.DataInstructions.Instructions.Interfaces;
    using Microsoft.EntityFrameworkCore;

    public class ReceivingInstruction<TEntity, TId> : IOperationInstruction<TEntity>
        where TEntity : class, IEntityWithId<TId>, new()
    {
        private readonly DbContext context;
        private readonly TId id;
        private readonly string[] navigationProperties = null;

        public ReceivingInstruction(DbContext context, TId id)
        {
            this.context = context;
            this.id = id;
        }

        public ReceivingInstruction(DbContext context, TId id, string[] navigationProperties): this(context, id)
        {
            this.navigationProperties = navigationProperties;
        }

        public async Task<TEntity> Execute()
        {
            var dbSet = this.context.Set<TEntity>();
            IQueryable<TEntity> items = dbSet;

            if (this.navigationProperties != null && this.navigationProperties.Length > 0)
            {
                foreach (var navProp in this.navigationProperties)
                {
                    items = items.Include(navProp);
                }
            }

            var entity = await items.SingleOrDefaultAsync<TEntity>(x => x.Id.Equals(this.id));

            return entity;
        }
    }
}
