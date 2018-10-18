namespace EEFApps.ApiInstructions.DataInstructions.Instructions.Interfaces
{
    using System.Threading.Tasks;

    interface IContinuedActionInstruction<TResult> : IOperationInstruction<TResult>
    {
        Task BuildAction();
    }
}
