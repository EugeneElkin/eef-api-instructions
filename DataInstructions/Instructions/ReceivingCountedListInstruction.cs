namespace EEFApps.ApiInstructions.DataInstructions.Instructions
{
    using System.Threading.Tasks;
    using EEFApps.ApiInstructions.DataInstructions.Instructions.Interfaces;
    using EEFApps.ApiInstructions.DataInstructions.Instructions.Structures;
    using Microsoft.EntityFrameworkCore;

    public class ReceivingCountedListInstruction<TEntity> : IOperationInstruction<int>
        where TEntity : class, new()
    {
        private readonly DbContext context;
        private readonly ListInstructionParams<TEntity> options;

        public ReceivingCountedListInstruction(DbContext context, ListInstructionParams<TEntity> options)
        {
            this.context = context;
            this.options = options;
        }

        public async Task<int> Execute()
        {
            var instruction = await new ReceivingListInstruction<TEntity>(this.context, this.options).GetInstruction();
            return await instruction.CountAsync();
        }
    }
}
