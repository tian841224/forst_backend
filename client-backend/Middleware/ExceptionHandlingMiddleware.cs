using CommonLibrary.DTOs;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text.Json;

namespace CommonLibrary.Middleware
{
    public class ExceptionHandling
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandling> _logger;
        public ExceptionHandling(RequestDelegate next, ILogger<ExceptionHandling> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json"; //回傳類型
            var response = context.Response;

            var errorResponse = new APIResult
            {
                IsSuccess = false,
                ErrorMsg = exception.Message,
            };

            if (exception is ArgumentNullException || exception is ArgumentException)
            {
                response.StatusCode = (int)HttpStatusCode.BadRequest; // 400
            }
            else if (exception is UnauthorizedAccessException)
            {
                response.StatusCode = (int)HttpStatusCode.Unauthorized; // 401
            }
            else if (exception is NotImplementedException)
            {
                response.StatusCode = (int)HttpStatusCode.NotImplemented; // 501
            }
            else
            {
                response.StatusCode = (int)HttpStatusCode.InternalServerError; // 500
            }

            _logger.LogError(exception.Message);

            var result = JsonSerializer.Serialize(errorResponse);
            await context.Response.WriteAsync(result);
        }
    }
}
