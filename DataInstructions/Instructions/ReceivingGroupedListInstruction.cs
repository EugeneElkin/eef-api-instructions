namespace EEFApps.ApiInstructions.DataInstructions.Instructions
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using EEFApps.ApiInstructions.BaseEntities.Entities.Interfaces;
    using EEFApps.ApiInstructions.DataInstructions.Instructions.Interfaces;
    using EEFApps.ApiInstructions.DataInstructions.Instructions.Structures;
    using Microsoft.EntityFrameworkCore;

    public class ReceivingGroupedListInstruction<TEntity> : IOperationInstruction<IEnumerable<GroupedItem>>
         where TEntity : class, IVersionedEntity, new()
    {
        private readonly DbContext context;
        private readonly GroupedListInstructionParams<TEntity> options;

        public ReceivingGroupedListInstruction(DbContext context, GroupedListInstructionParams<TEntity> options)
        {
            this.context = context;
            this.options = options;
        }

        public async Task<IEnumerable<GroupedItem>> Execute()
        {
            var instruction = await new ReceivingListInstruction<TEntity>(this.context, this.options).GetInstruction();
            var vgroupedItems = instruction.GroupBy(this.options.GroupExpr).Select((g) => new GroupedItem { Key = g.Key, AggreagtionResult = this.Aggreagate(g, this.options.aggregationType) });
            return vgroupedItems.ToList();
        }

        private int Aggreagate(IGrouping<string, TEntity> grouping, AggregationType type)
        {
            // TODO: Implement different types in future. Probably grouping aggregation expression is necessery to pass here too
            return grouping.Count();
        }
    }
}
