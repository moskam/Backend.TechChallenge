using Microsoft.AspNetCore.Http;
using Serilog;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace Backend.TechChallenge.Api.Middleware
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public ErrorHandlerMiddleware(RequestDelegate next, ILogger logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                // in prod environment we should not expose stacktrace of the code, in test enviroments could be exposed. This can be handled in the configuration.
                // most important to log error, if hiding it's origin.

                var message = "Unexpected error occurred";
                var response = context.Response;
                var result = JsonSerializer.Serialize(new { Message = message });

                _logger.Error(ex, message);
                response.ContentType = "application/json";
                response.StatusCode = (int)HttpStatusCode.InternalServerError;

                await response.WriteAsync(result);
            }
        }
    }
}
