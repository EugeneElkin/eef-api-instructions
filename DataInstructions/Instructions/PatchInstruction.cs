namespace DataInstructions.Instructions
{
    using System.Linq;
    using System.Threading.Tasks;
    using BaseEntities.Entities;
    using DataInstructions.Instructions.Interfaces;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.AspNetCore.JsonPatch;

    public class PatchInstruction<TEntity, TId> : IOperationInstruction<bool>
        where TEntity : class, IEntityWithId<TId>, new()
    {
        private readonly DbContext context;
        private readonly TId id;
        private readonly JsonPatchDocument<TEntity> deltaEntity = null;
        private readonly string[] navigationProperties;

        public PatchInstruction(DbContext context, TId id, JsonPatchDocument<TEntity> deltaEntity)
        {
            this.context = context;
            this.id = id;
            this.deltaEntity = deltaEntity;
        }

        public PatchInstruction(DbContext context, TId id, JsonPatchDocument<TEntity> deltaEntity, string[] navigationProperties): this(context, id, deltaEntity)
        {
            this.navigationProperties = navigationProperties;
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

            var targetEntity = await items.SingleOrDefaultAsync<TEntity>(x => x.Id.Equals(this.id));
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
