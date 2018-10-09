namespace EEFApps.ApiInstructions.DataInstructions.Exceptions
{
    using System;
    using System.Globalization;
    using System.Net;

    public class InstructionException : Exception
    {
        public HttpStatusCode httpStatusCode = HttpStatusCode.BadRequest;

        public InstructionException() : base() { }

        public InstructionException(string message, HttpStatusCode httpStatusCode) : base(message) {
            this.httpStatusCode = httpStatusCode;
        }

        public InstructionException(string message, HttpStatusCode httpStatusCode, params object[] args)
            : base(String.Format(CultureInfo.CurrentCulture, message, args))
        {
            this.httpStatusCode = httpStatusCode;
        }
    }
}
