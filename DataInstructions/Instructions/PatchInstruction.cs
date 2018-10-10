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
    using Microsoft.AspNetCore.JsonPatch;
    
    public class PatchInstruction<TEntity, TId> : IOperationInstruction<bool>
        where TEntity : class, IEntityWithId<TId>, new()
    {
        private readonly DbContext context;
        private readonly TId id;
        private readonly JsonPatchDocument<TEntity> deltaEntity = null;
        private readonly string[] navigationProperties;
        private readonly Expression<Func<TEntity, bool>> filterExpr = null;

        public PatchInstruction(DbContext context, PatchInstructionParams<TEntity, TId> options)
        {
            this.context = context;
            this.id = options.Id;
            this.deltaEntity = options.DeltaEntity;
            this.navigationProperties = options.NavigationProperties;
            this.filterExpr = x => x.Id.Equals(this.id);
        }

        protected PatchInstruction(DbContext context, PatchInstructionParams<TEntity, TId> options, Expression<Func<TEntity, bool>> filterExpr): this(context, options)
        {
            this.filterExpr = filterExpr;
        }

        public async Task<bool> Execute()
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

            var targetEntity = await items.SingleOrDefaultAsync<TEntity>(this.filterExpr);
            if (targetEntity == null)
            {
                throw new InstructionException("The resource wasn't found!", HttpStatusCode.NotFound);
            }

            this.deltaEntity.ApplyTo(targetEntity);

            try
            {
                await this.context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!items.Any(e => e.Id.Equals(id)))
                {
                    return false;
                }
                else
                {
                    throw;
                }
            }

            return true;
        }
    }
}
