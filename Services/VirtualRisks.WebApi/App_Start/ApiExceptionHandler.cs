using CastleGo.WebApi.Models;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Results;

namespace CastleGo.WebApi
{
    /// <summary>
    /// Represents implementation of <see cref="T:System.Web.Http.ExceptionHandling.ExceptionHandler" />.
    /// </summary>
    public class ApiExceptionHandler : ExceptionHandler
    {
        /// <summary>
        /// Overrides <see cref="M:System.Web.Http.ExceptionHandling.ExceptionHandler.Handle(System.Web.Http.ExceptionHandling.ExceptionHandlerContext)" /> method with code that sets friendly error message to be shown in browser.
        /// </summary>
        /// <param name="context">Instance fo <see cref="T:System.Web.Http.ExceptionHandling.ExceptionHandlerContext" />.</param>
        public override void Handle(ExceptionHandlerContext context)
        {
            ErrorInfoModel errorInfoModel = new ErrorInfoModel
            {
                Message = "An unexpected error occurred! Please use the Error ID to contact support",
                TimeStamp = DateTime.UtcNow,
                RequestUri = context.Request.RequestUri,
                ErrorId = Guid.NewGuid(),
                ExeptionMessage = context.Exception.Message
            };
            HttpResponseMessage response = context.Request.CreateResponse(HttpStatusCode.InternalServerError, errorInfoModel);
            context.Result = new ResponseMessageResult(response);
        }
    }
}
