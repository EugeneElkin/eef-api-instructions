namespace EEFApps.ApiInstructions.DataInstructions.Instructions.Structures
{
    using System;
    using System.Linq.Expressions;

    public class ListInstructionParams<TEntity>
        where TEntity : class
    {
        public Expression<Func<TEntity, bool>> FilterExpr { get; set; }
        public string OrderByField { get; set; }
        public bool IsDescending { get; set; }
        public int? PageSize { get; set; }
        public int? PageAt { get; set; }
        public string[] NavigationProperties { get; set; }
    }
}
