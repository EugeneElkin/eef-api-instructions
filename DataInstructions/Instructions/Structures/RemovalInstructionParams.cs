namespace EEFApps.ApiInstructions.DataInstructions.Instructions.Structures
{
    using System;
    using System.Net;
    using EEFApps.ApiInstructions.DataInstructions.Exceptions;

    public class RemovalInstructionParams<TId>
    {
        public TId Id { get; set; }
        public string UserId { get; set; }

        private byte[] rowVersion;
        public byte[] RowVersion
        {
            get
            {
                return this.rowVersion;
            }
        }

        private string base64RowVersion;
        public string Base64RowVersion
        {
            get {
                return this.base64RowVersion;
            }
            set {
                try
                {
                    this.rowVersion = value != null ? Convert.FromBase64String(value) : null;
                    this.base64RowVersion = value;
                }
                catch(FormatException)
                {
                    throw new InstructionException("Row version format is incorrect! It must be Base-64 char array or string!", HttpStatusCode.BadRequest);
                }
            }
        }
        
        
    }
}
