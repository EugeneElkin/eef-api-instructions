namespace EEFApps.ApiInstructions.DataInstructions.Instructions.Structures
{
    using Microsoft.AspNetCore.JsonPatch;

    public class PatchInstructionParams<TEntity, TId>
        where TEntity : class
    {
        public TId id;
        public JsonPatchDocument<TEntity> deltaEntity = null;
        public string[] navigationProperties;
    }
}
