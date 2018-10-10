namespace EEFApps.ApiInstructions.DataInstructions.Instructions.Structures
{
    using Microsoft.AspNetCore.JsonPatch;

    public class PatchInstructionParams<TEntity, TId>
        where TEntity : class
    {
        public TId Id { get; set; }
        public JsonPatchDocument<TEntity> DeltaEntity { get; set; }
        public string[] NavigationProperties { get; set; }
    }
}
