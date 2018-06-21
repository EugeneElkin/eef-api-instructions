namespace DataInstructions.Instructions.Interfaces
{
    using System.Threading.Tasks;

    public interface IOperationInstruction<TResult>
    {
        Task<TResult> Execute();
    }
}
