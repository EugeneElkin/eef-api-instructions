namespace EEFApps.ApiInstructions.DataInstructions.Instructions.Structures
{
    using System;
    using System.Linq.Expressions;

    public class ReceivingInstructionParams<TEntity, TId>
    {
        public Expression<Func<TEntity, bool>> FilterExpr { get; set; }
        public TId Id { get; set; }
        public string[] NavigationProperties { get; set; }
    }
}
