namespace EEFApps.ApiInstructions.DataInstructions.Instructions
{
    using System;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using EEFApps.ApiInstructions.BaseEntities.Entities;
    using EEFApps.ApiInstructions.DataInstructions.Instructions.Interfaces;
    using EEFApps.ApiInstructions.DataInstructions.Instructions.Structures;
    using Microsoft.EntityFrameworkCore;

    public class RemovalOptimizedInstruction<TEntity, TId> : IOperationInstruction<bool>
        where TEntity : BaseEntity<TId>, new()
    {
        private readonly DbContext context;
        private readonly TId id;
        private readonly byte[] rowVersion;

        public RemovalOptimizedInstruction(DbContext context, RemovalInstructionParams<TId> options)
        {
            this.context = context;
            this.id = options.id;
            this.rowVersion = Convert.FromBase64String(options.base64rowVersion);
        }

        public async Task<bool> Execute()
        {
            TEntity entity = new TEntity() { Id = this.id, RowVersion = this.rowVersion };
            var dbSet = this.context.Set<TEntity>();   
            dbSet.Attach(entity);

            if (entity != null)
            {
                this.context.Remove<TEntity>(entity);
                await this.context.SaveChangesAsync();
                return true;
            }

            return false;
        }
    }
}
