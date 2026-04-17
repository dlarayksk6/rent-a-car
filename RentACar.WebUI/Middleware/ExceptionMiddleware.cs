using System.Net;
using System.Text.Json;

namespace RentACar.WebUI.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IWebHostEnvironment _env;

        public ExceptionMiddleware(
            RequestDelegate next,
            ILogger<ExceptionMiddleware> logger,
            IWebHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Beklenmeyen hata: {Message} | Path: {Path} | User: {User}",
                    ex.Message,
                    context.Request.Path,
                    context.User?.Identity?.Name ?? "Anonim");

                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            if (context.Request.Path.StartsWithSegments("/api"))
            {
                context.Response.ContentType = "application/json";
                var response = _env.IsDevelopment()
                    ? new { error = ex.Message, detail = ex.StackTrace }
                    : new { error = "Sunucu hatası oluştu.", detail = "" };
                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
            }
            else
            {
                context.Response.Redirect("/Home/Error?statusCode=500");
            }
        }
    }
}