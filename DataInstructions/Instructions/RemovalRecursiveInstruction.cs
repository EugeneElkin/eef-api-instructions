namespace EEFApps.ApiInstructions.DataInstructions.Instructions
{
    using System;
    using System.Linq.Expressions;
    using System.Net;
    using System.Threading.Tasks;
    using EEFApps.ApiInstructions.BaseEntities.Entities.Interfaces;
    using EEFApps.ApiInstructions.DataInstructions.Exceptions;
    using EEFApps.ApiInstructions.DataInstructions.Instructions.Interfaces;
    using EEFApps.ApiInstructions.DataInstructions.Instructions.Structures;
    using Microsoft.EntityFrameworkCore;

    public class RemovalRecursiveInstruction<TEntity, TId> : IOperationInstruction<bool>
        where TEntity : class, IEntityWithId<TId>, IEntityWithParent<TId>, new()
    {
        private readonly DbContext context;
        private readonly TId id;
        private readonly Expression<Func<TEntity, bool>> filterExpr = null;

        public RemovalRecursiveInstruction(DbContext context, RemovalInstructionParams<TId> options)
        {
            this.context = context;
            this.id = options.Id;
            this.filterExpr = x => x.Id.Equals(this.id);
        }

        protected RemovalRecursiveInstruction(DbContext context, RemovalInstructionParams<TId> options, Expression<Func<TEntity, bool>> filterExpr) : this(context, options)
        {
            this.filterExpr = filterExpr;
        }

        public async Task<bool> Execute()
        {
            var entity = await new ReceivingInstruction<TEntity, TId>(
                this.context,
                new ReceivingInstructionParams<TId>
                {
                    Id = this.id
                },
                this.filterExpr)
                .Execute();

            if (entity == null)
            {
                throw new InstructionException("The resource wasn't found!", HttpStatusCode.NotFound);
            }

            await this.RecursiveRemoval(entity);

            await this.context.SaveChangesAsync();

            return true;
        }

        private async Task RecursiveRemoval(TEntity entity)
        {
            var children = await new ReceivingListInstruction<TEntity>(
                this.context,
                new ListInstructionParams<TEntity>
                {
                    FilterExpr = x => x.ParentId.Equals(entity.Id)
                })
                .Execute();

            if (children != null)
            {
                foreach (var child in children)
                {
                    await this.RecursiveRemoval(child);
                }
            }

            await new RemovalInstruction<TEntity, TId>(
                this.context,
                new RemovalInstructionParams<TId>() { Id = entity.Id }).BuildAction();
        }
    }
}
