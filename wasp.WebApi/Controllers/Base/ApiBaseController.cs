using System;
using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using wasp.WebApi.Data.Dto.Base;

namespace wasp.WebApi.Controllers.Base
{
    public abstract class ApiBaseController : ControllerBase
    {
        public EnvelopeResult EnvelopeResult { get; init; }
        
        public ApiBaseController()
        {
            EnvelopeResult = new EnvelopeResult(this);
        }
        
        [NonAction]
        public StatusCodeResult Forbidden()
            => StatusCode((int) HttpStatusCode.Forbidden);

        [NonAction]
        public StatusCodeResult InternalServerError() =>
            StatusCode((int) HttpStatusCode.InternalServerError);

        [NonAction]
        protected IActionResult Error(int httpCode, Error error)
        {
            if (httpCode is < 400 or >= 600)
            {
                throw new ArgumentException(
                    $"The passed {nameof(httpCode)} argument \"{httpCode}\" is NOT a valid HTTP Status code from the error code range!",
                    nameof(httpCode));
            }

            return StatusCode(httpCode, new ResponseBodyDto<object> {Error = error});
        }

        [NonAction]
        protected IActionResult Error(HttpStatusCode httpCode, Error error)
        {
            return Error((int) httpCode, error);
        }

        [NonAction]
        protected IActionResult Ok<T>(long totalItems, IEnumerable<T> items) where T : class
        {
            items ??= Array.Empty<T>();

            return Ok(new ResponseBodyDto<T>
            {
                Data = new ResponseBodyDataDto<T>
                {
                    Count = totalItems,
                    Type = typeof(T).Name,
                    Items = items,
                }
            });
        }

        [NonAction]
        protected IActionResult Created<T>(string uri, long totalItems, IEnumerable<T> items) where T : class
        {
            return Created(uri, new ResponseBodyDto<T>
            {
                Data = new ResponseBodyDataDto<T>
                {
                    Count = totalItems,
                    Type = typeof(T).Name,
                    Items = items
                }
            });
        }

        [NonAction]
        protected IActionResult Updated<T>(long totalItems, T item) where T : class
        {
            return Ok(totalItems, new[] {item});
        }
    }
}