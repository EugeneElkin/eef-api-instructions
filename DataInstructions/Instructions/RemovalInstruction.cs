namespace EEFApps.ApiInstructions.DataInstructions.Instructions
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Net;
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
            this.id = options.Id;
            this.filterExpr = x => x.Id.Equals(this.id);
        }

        protected RemovalInstruction(DbContext context, RemovalInstructionParams<TId> options, Expression<Func<TEntity, bool>> filterExpr) : this(context, options)
        {
            this.filterExpr = filterExpr;
        }

        public async Task<bool> Execute()
        {
            IQueryable<TEntity> dbSet = this.context.Set<TEntity>();
            var targetEntity = await dbSet.SingleOrDefaultAsync<TEntity>(this.filterExpr);

            if (targetEntity == null)
            {
                throw new InstructionException("The resource wasn't found!", HttpStatusCode.NotFound);
            }

            this.context.Remove<TEntity>(targetEntity);

            try
            {
                await this.context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
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
