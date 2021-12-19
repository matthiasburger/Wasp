using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace wasp.WebApi.Data.Dto.Base
{
    public class ResponseBodyDto<T>
    {
        /// <summary>
        /// Response data payload.
        /// </summary>
        public ResponseBodyDataDto<T> Data { get; set; }

        /// <summary>
        /// If the request failed, the error should be written into this field for the client to handle.
        /// </summary>
        public Error Error { get; set; }

        /// <summary>
        /// Parameterless ctor.
        /// </summary>
        public ResponseBodyDto()
        {
            //nop
        }

        /// <summary>
        /// ActionContext ctor for Validation-ErrorHandling 
        /// </summary>
        public ResponseBodyDto(ActionContext actionContext)
        {
            List<string> validationErrors = actionContext.ModelState
                .SelectMany(modelError =>
                    modelError.Value.Errors.Select(x => x.ErrorMessage)
                ).ToList();

            Error = new Error((int)HttpStatusCode.BadRequest,
                $"{validationErrors.Count} validation-errors. Check 'Errors' section")
            {
                Errors = validationErrors.Select(c => new ErrorDetail { Message = c }).ToList()
            };
        }
    }
}