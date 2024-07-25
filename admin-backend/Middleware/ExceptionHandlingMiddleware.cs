using CommonLibrary.DTOs;
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

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // Capture the original response stream
                var originalResponseBodyStream = context.Response.Body;

                using (var responseBody = new MemoryStream())
                {
                    context.Response.Body = responseBody;

                    await _next(context);

                    context.Response.Body = originalResponseBodyStream;

                    // Process the response
                    if (context.Response.StatusCode == StatusCodes.Status200OK)
                    {
                        responseBody.Seek(0, SeekOrigin.Begin);
                        var responseBodyText = await new StreamReader(responseBody).ReadToEndAsync();

                        var apiResult = new APIResult
                        {
                            Success = true,
                        };

                        // Check if the response body is empty
                        if (!string.IsNullOrWhiteSpace(responseBodyText))
                        {

                            var data = JsonSerializer.Deserialize<object>(responseBodyText);
                            apiResult.Data = data;
                        }

                        var jsonResult = JsonSerializer.Serialize(apiResult);

                        context.Response.ContentType = "application/json";
                        await context.Response.WriteAsync(jsonResult);
                    }
                    else
                    {
                        responseBody.Seek(0, SeekOrigin.Begin);
                        await responseBody.CopyToAsync(originalResponseBodyStream);
                    }
                }
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json"; //回傳類型
            var response = context.Response;

            var errorResponse = new APIResult
            {
                Success = false,
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
