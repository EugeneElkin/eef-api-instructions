namespace ApiInstructions.DataInstructions.Instructions.Structures
{
    using System;
    using System.Linq.Expressions;

    public class ListInstructionParams<TEntity>
        where TEntity : class
    {
        public Expression<Func<TEntity, bool>> filterExpr = null;
        public string orderByField = null;
        public bool isDescending = false;
        public int? pageSize = null;
        public int? pageAt = null;
        public string[] navigationProperties = null;
    }
}
