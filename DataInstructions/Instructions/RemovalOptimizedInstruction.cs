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
            this.id = options.Id;
            this.rowVersion = options.RowVersion;
        }

        protected RemovalOptimizedInstruction(DbContext context, RemovalInstructionParams<TId> options, TEntity entity) : this(context, options)
        {
            this.entity = entity;
        }

        public async Task<bool> Execute()
        {
            // Since the instruction uses optimized approach for removal,
            // it is necessery to provide row version to attch exact record of the entity.
            TEntity targetEntity = this.entity ?? new TEntity() { Id = this.id, RowVersion = this.rowVersion };
            var dbSet = this.context.Set<TEntity>();
            dbSet.Attach(targetEntity);           
            this.context.Remove<TEntity>(targetEntity);

            try
            {
                await this.context.SaveChangesAsync();
            }
            catch(DbUpdateConcurrencyException)
            {
                // If row version is wrong or entity ID doesn't exist DbUpdateConcurrencyException will be generated
                // because the entity was connected by attach method with an inexistent record.
                throw new InstructionException("The resource wasn't found! The resource may have been modified or deleted since it was loaded!", HttpStatusCode.NotFound);
            }
            catch(DbUpdateException ex)
            {
                if (ex.InnerException != null && ex.InnerException.Message.Contains("SAME TABLE REFERENCE"))
                {
                    throw new InstructionException("The resource contains child resources of the same type that refer to a parent resource too! This operation requires to use recursive deletion instruction instead!", HttpStatusCode.BadRequest);
                }
                
                throw;
            }

            return true;
        }
    }
}
