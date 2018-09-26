namespace EEFApps.ApiInstructions.DataInstructions.Instructions
{
    using System;
    using System.Threading.Tasks;
    using EEFApps.ApiInstructions.BaseEntities.Entities;
    using EEFApps.ApiInstructions.DataInstructions.Instructions.Interfaces;
    using Microsoft.EntityFrameworkCore;

    public class RemovalInstruction<TEntity, TId> : IOperationInstruction<bool>
        where TEntity : BaseEntity<TId>, new()
    {
        private readonly DbContext context;
        private readonly TId id;
        private readonly byte[] rowVersion;

        /// <summary>
        /// Removal instruction contructor
        /// </summary>
        /// <param name="context">Db context</param>
        /// <param name="id">Id of target entity</param>
        /// <param name="rowVersion">base64 encoded string on byte array to solve concurrency issues</param>
        public RemovalInstruction(DbContext context, TId id, string base64rowVersion)
        {
            this.context = context;
            this.id = id;
            this.rowVersion = Convert.FromBase64String(base64rowVersion);
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
