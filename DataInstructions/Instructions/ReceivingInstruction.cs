namespace EEFApps.ApiInstructions.DataInstructions.Instructions
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using EEFApps.ApiInstructions.BaseEntities.Entities.Interfaces;
    using EEFApps.ApiInstructions.DataInstructions.Instructions.Interfaces;
    using EEFApps.ApiInstructions.DataInstructions.Instructions.Structures;
    using Microsoft.EntityFrameworkCore;

    public class ReceivingInstruction<TEntity, TId> : IOperationInstruction<TEntity>
        where TEntity : class, IEntityWithId<TId>, new()
    {
        private readonly DbContext context;
        private readonly TId id;
        private readonly string[] navigationProperties = null;
        private readonly Expression<Func<TEntity, bool>> filterExpr = null;

        public ReceivingInstruction(DbContext context, ReceivingInstructionParams<TId> options)
        {
            this.context = context;
            this.id = options.Id;
            this.navigationProperties = options.NavigationProperties;
            this.filterExpr = x => x.Id.Equals(this.id);
        }

        protected internal ReceivingInstruction(DbContext context, ReceivingInstructionParams<TId> options, Expression<Func<TEntity, bool>> filterExpr) : this(context, options)
        {
            this.filterExpr = filterExpr;
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

            var entity = await items.SingleOrDefaultAsync<TEntity>(this.filterExpr);

            return entity;
        }
    }
}
