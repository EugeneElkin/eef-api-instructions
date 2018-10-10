namespace EEFApps.ApiInstructions.DataInstructions.Instructions.Structures
{
    public class ReceivingInstructionParams<TId>
    {
        public TId Id { get; set; }
        public string[] NavigationProperties { get; set; }
    }
}
