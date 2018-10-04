namespace EEFApps.ApiInstructions.DataInstructions.Exceptions
{
    using System;
    using System.Globalization;

    public class InstructionException : Exception
    {
        public InstructionException() : base() { }

        public InstructionException(string message) : base(message) { }

        public InstructionException(string message, params object[] args)
            : base(String.Format(CultureInfo.CurrentCulture, message, args))
        {
        }
    }
}
