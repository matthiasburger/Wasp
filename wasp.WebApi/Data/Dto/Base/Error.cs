using System.Collections.Generic;

namespace wasp.WebApi.Data.Dto.Base
{
    public class ErrorDetail
    {
        public string Message { get; set; }
    }
    
    /// <summary>
    /// Error structure to return in response bodies.
    /// </summary>
    public class Error
    {
        /// <summary>
        /// The error code (clients can switch on this number to for example pick a corresponding translated error message).
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// Raw server-side error message to the client (in English).
        /// </summary>
        public string Message { get; set; }
        
        /// <summary>
        /// If the request failed, the error should be written into this field for the client to handle.
        /// </summary>
        public List<ErrorDetail> Errors { get; set; }

        /// <summary>
        /// Parameterless ctor.
        /// </summary>
        public Error()
        {
            //nop
        }

        /// <summary>
        /// Creates an <see cref="Error"/> using a specific error code and message.
        /// </summary>
        /// <param name="code">Internal API error code.</param>
        /// <param name="message">Server-side error message (in English).</param>
        public Error(int code, string message)
        {
            Code = code;
            Message = message;
        }
    }
}