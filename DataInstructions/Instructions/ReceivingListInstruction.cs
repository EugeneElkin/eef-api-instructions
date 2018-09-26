namespace EEFApps.ApiInstructions.DataInstructions.Instructions
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using EEFApps.ApiInstructions.BaseEntities.Entities.Interfaces;
    using EEFApps.ApiInstructions.DataInstructions.Extensions;
    using EEFApps.ApiInstructions.DataInstructions.Instructions.Interfaces;
    using EEFApps.ApiInstructions.DataInstructions.Instructions.Structures;
    using Microsoft.EntityFrameworkCore;

    public class ReceivingListInstruction<TEntity> : IContinuedInstruction<IEnumerable<TEntity>, TEntity>
        where TEntity : class, IVersionedEntity, new()
    {
        private readonly DbContext context;
        private readonly ListInstructionParams<TEntity> options;

        public ReceivingListInstruction(DbContext context, ListInstructionParams<TEntity> options)
        {
            this.context = context;
            this.options = options;
        }

        public async Task<IQueryable<TEntity>> GetInstruction()
        {
            var dbSet = this.context.Set<TEntity>();
            IQueryable<TEntity> items = dbSet;

            if (this.options.navigationProperties != null && this.options.navigationProperties.Length > 0)
            {
                foreach (var navProp in this.options.navigationProperties)
                {
                    items = items.Include(navProp);
                }
            }

            if (!string.IsNullOrEmpty(this.options.orderByField))
            {
                items = this.options.isDescending ? items.OrderByDescending(this.options.orderByField) : items.OrderBy(this.options.orderByField);
            }

            if (this.options.filterExpr != null)
            {
                items = items.Where(this.options.filterExpr);
            }

            if (this.options.pageAt.HasValue && this.options.pageSize.HasValue)
            {
                items = items.Skip(this.options.pageSize.Value * (this.options.pageAt.Value - 1)).Take(this.options.pageSize.Value);
            }

            return await Task.FromResult(items);
        }

        public async Task<IEnumerable<TEntity>> Execute()
        {
            var items = await this.GetInstruction();

            return items.ToList();
        }

        
    }
}
