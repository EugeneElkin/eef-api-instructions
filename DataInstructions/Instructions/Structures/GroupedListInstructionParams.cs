namespace EEFApps.ApiInstructions.DataInstructions.Instructions.Structures
{
    using System;
    using System.Linq.Expressions;

    public class GroupedListInstructionParams<TEntity> : ListInstructionParams<TEntity>
        where TEntity : class
    {
        public Expression<Func<TEntity, string>> groupExpr = null;
        public AggregationType aggregationType = AggregationType.Count;
    }
}
