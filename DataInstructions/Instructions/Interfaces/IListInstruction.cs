namespace DataInstructions.Instructions.Interfaces
{
    using System.Linq;
    using System.Threading.Tasks;

    public interface IContinuedInstruction<TResult, TEntity> : IOperationInstruction<TResult>
    {
        Task<IQueryable<TEntity>> GetInstruction();
    }
}
