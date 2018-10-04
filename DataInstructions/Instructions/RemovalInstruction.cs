namespace EEFApps.ApiInstructions.DataInstructions.Instructions
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using EEFApps.ApiInstructions.BaseEntities.Entities;
    using EEFApps.ApiInstructions.DataInstructions.Exceptions;
    using EEFApps.ApiInstructions.DataInstructions.Instructions.Interfaces;
    using EEFApps.ApiInstructions.DataInstructions.Instructions.Structures;
    using Microsoft.EntityFrameworkCore;

    public class RemovalInstruction<TEntity, TId> : IOperationInstruction<bool>
        where TEntity : BaseEntity<TId>, new()
    {
        private readonly DbContext context;
        private readonly TId id;
        private readonly Expression<Func<TEntity, bool>> filterExpr = null;

        public RemovalInstruction(DbContext context, RemovalInstructionParams<TId> options)
        {
            this.context = context;
            this.id = options.id;
            this.filterExpr = x => x.Id.Equals(this.id);
        }

        public async Task<bool> Execute()
        {
            IQueryable<TEntity> dbSet = this.context.Set<TEntity>();
            var targetEntity = await dbSet.SingleOrDefaultAsync<TEntity>(x => x.Id.Equals(this.id));

            if (targetEntity == null)
            {
                throw new InstructionException("Target entity wasn't found!");
            }

            this.context.Remove<TEntity>(targetEntity);
            await this.context.SaveChangesAsync();

            return true;
        }
    }
}
