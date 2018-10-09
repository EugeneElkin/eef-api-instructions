namespace EEFApps.ApiInstructions.DataInstructions.Instructions
{
    using System;
    using System.Net;
    using System.Threading.Tasks;
    using EEFApps.ApiInstructions.BaseEntities.Entities;
    using EEFApps.ApiInstructions.DataInstructions.Exceptions;
    using EEFApps.ApiInstructions.DataInstructions.Instructions.Interfaces;
    using EEFApps.ApiInstructions.DataInstructions.Instructions.Structures;
    using Microsoft.EntityFrameworkCore;

    public class RemovalOptimizedInstruction<TEntity, TId> : IOperationInstruction<bool>
        where TEntity : BaseEntity<TId>, new()
    {
        private readonly DbContext context;
        private readonly TId id;
        private readonly byte[] rowVersion;
        private readonly TEntity entity = null;

        public RemovalOptimizedInstruction(DbContext context, RemovalInstructionParams<TId> options)
        {
            this.context = context;
            this.id = options.id;
            this.rowVersion = Convert.FromBase64String(options.base64rowVersion);
        }

        protected RemovalOptimizedInstruction(DbContext context, RemovalInstructionParams<TId> options, TEntity entity) : this(context, options)
        {
            this.entity = entity;
        }

        public async Task<bool> Execute()
        {
            TEntity targetEntity = this.entity ?? new TEntity() { Id = this.id, RowVersion = this.rowVersion };
            var dbSet = this.context.Set<TEntity>();
            dbSet.Attach(targetEntity);

            // TODO: Catch and handle an issue about concurency because of different row version
            // Solution: it is sense to handle like not found because in fact it is true
            if (targetEntity == null)
            {
                throw new InstructionException("The resource wasn't found!", HttpStatusCode.NotFound);
            }

            this.context.Remove<TEntity>(targetEntity);
            await this.context.SaveChangesAsync();

            return true;
        }
    }
}
