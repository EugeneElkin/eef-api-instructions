namespace EEFApps.ApiInstructions.DataInstructions.Instructions
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Net;
    using System.Threading.Tasks;
    using EEFApps.ApiInstructions.BaseEntities.Entities.Interfaces;
    using EEFApps.ApiInstructions.DataInstructions.Exceptions;
    using EEFApps.ApiInstructions.DataInstructions.Instructions.Interfaces;
    using EEFApps.ApiInstructions.DataInstructions.Instructions.Structures;
    using Microsoft.EntityFrameworkCore;
    using global::DataInstructions.Services;

    public class ReceivingInstruction<TEntity, TId> : IOperationInstruction<TEntity>
        where TEntity : class, IEntityWithId<TId>, new()
    {
        private readonly DbContext context;
        private readonly ReceivingInstructionParams<TEntity, TId> options;

        public ReceivingInstruction(DbContext context, ReceivingInstructionParams<TEntity, TId> options)
        {
            this.context = context;
            this.options = options;
            this.options.FilterExpr = this.CombineFilters(x => x.Id.Equals(this.options.Id));
        }

        protected internal ReceivingInstruction(DbContext context, ReceivingInstructionParams<TEntity, TId> options, Expression<Func<TEntity, bool>> filterExpr) : this(context, options)
        {
            this.options.FilterExpr = this.CombineFilters(filterExpr);
        }

        private Expression<Func<TEntity, bool>> CombineFilters(Expression<Func<TEntity, bool>> filterExpr1)
        {
            var filterExpr2 = this.options.FilterExpr;
            var combinedFilters = Expression.Lambda<Func<TEntity, bool>>(Expression.AndAlso(
            new SwapVisitor(filterExpr1.Parameters[0], filterExpr2.Parameters[0]).Visit(filterExpr1.Body),
            filterExpr2.Body), filterExpr2.Parameters);

            return combinedFilters;
        }

        public async Task<TEntity> Execute()
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

            var entity = await items.SingleOrDefaultAsync<TEntity>(this.options.FilterExpr);

            if (entity == null)
            {
                throw new InstructionException("The resource wasn't found!", HttpStatusCode.NotFound);
            }

            return entity;
        }
    }
}
