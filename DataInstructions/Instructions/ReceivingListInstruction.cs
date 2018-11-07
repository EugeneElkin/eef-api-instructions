namespace EEFApps.ApiInstructions.DataInstructions.Instructions
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using EEFApps.ApiInstructions.DataInstructions.Extensions;
    using EEFApps.ApiInstructions.DataInstructions.Instructions.Interfaces;
    using EEFApps.ApiInstructions.DataInstructions.Instructions.Structures;
    using Microsoft.EntityFrameworkCore;

    public class ReceivingListInstruction<TEntity> : IContinuedInstruction<IEnumerable<TEntity>, TEntity>
        where TEntity : class, new()
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

            if (this.options.NavigationProperties != null && this.options.NavigationProperties.Length > 0)
            {
                foreach (var navProp in this.options.NavigationProperties)
                {
                    items = items.Include(navProp);
                }
            }

            if (!string.IsNullOrEmpty(this.options.OrderByField))
            {
                items = this.options.IsDescending ? items.OrderByDescending(this.options.OrderByField) : items.OrderBy(this.options.OrderByField);
            }

            if (this.options.FilterExpr != null)
            {
                items = items.Where(this.options.FilterExpr);
            }

            if (this.options.PageAt.HasValue && this.options.PageSize.HasValue)
            {
                items = items.Skip(this.options.PageSize.Value * (this.options.PageAt.Value - 1)).Take(this.options.PageSize.Value);
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
