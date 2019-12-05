using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace HexagonalArchitectureSample.WebApi.Filters
{
    public class MapExceptionToHttpStatusCodeFilterAttribute: ExceptionFilterAttribute
    {
        private readonly Type _exceptionType;
        private readonly int _statusCode;

        public MapExceptionToHttpStatusCodeFilterAttribute(Type exceptionType, int statusCode)
        {
            _exceptionType = exceptionType;
            _statusCode = statusCode;
        }

        public override void OnException(ExceptionContext context)
        {
            if (_exceptionType.IsInstanceOfType(context.Exception))
            {
                context.ExceptionHandled = true;
                context.Result = new StatusCodeResult(_statusCode);
            }
        }
    }
}
