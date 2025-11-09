using ApiClientes.Models;
using ApiClientes.Services;
using System.Text;
namespace ApiClientes.Middleware
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LoggingMiddleware> _logger;

        public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, ILogService logService)
        {
            var request = context.Request;
            string requestBody = string.Empty;
            string responseBody = string.Empty;

            // Leer request body (solo para contenido no multipart)
            if (request.ContentLength > 0 &&
                request.ContentLength < 1024 * 1024 &&
                !request.ContentType?.Contains("multipart/form-data", StringComparison.OrdinalIgnoreCase) == true)
            {
                request.EnableBuffering();
                using var reader = new StreamReader(request.Body, Encoding.UTF8, leaveOpen: true);
                requestBody = await reader.ReadToEndAsync();
                request.Body.Position = 0;
            }

            // Capturar response
            var originalBodyStream = context.Response.Body;
            using var responseBodyStream = new MemoryStream();
            context.Response.Body = responseBodyStream;

            try
            {
                await _next(context);

                responseBodyStream.Seek(0, SeekOrigin.Begin);
                responseBody = await new StreamReader(responseBodyStream).ReadToEndAsync();
                responseBodyStream.Seek(0, SeekOrigin.Begin);

                await responseBodyStream.CopyToAsync(originalBodyStream);

                // Registrar log exitoso
                await logService.RegistrarLogAsync(new LogApi
                {
                    TipoLog = context.Response.StatusCode >= 400 ? "WARNING" : "INFO",
                    RequestBody = requestBody.Length > 5000 ? requestBody.Substring(0, 5000) : requestBody,
                    ResponseBody = responseBody.Length > 5000 ? responseBody.Substring(0, 5000) : responseBody,
                    UrlEndpoint = $"{request.Scheme}://{request.Host}{request.Path}{request.QueryString}",
                    MetodoHttp = request.Method,
                    DireccionIp = context.Connection.RemoteIpAddress?.ToString(),
                    CodigoEstado = context.Response.StatusCode,
                    Detalle = "Solicitud procesada",
                    DateTime = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en el procesamiento de la solicitud");

                // Registrar error
                await logService.RegistrarLogAsync(new LogApi
                {
                    TipoLog = "ERROR",
                    RequestBody = requestBody,
                    ResponseBody = ex.Message,
                    UrlEndpoint = $"{request.Scheme}://{request.Host}{request.Path}{request.QueryString}",
                    MetodoHttp = request.Method,
                    DireccionIp = context.Connection.RemoteIpAddress?.ToString(),
                    CodigoEstado = 500,
                    Detalle = $"{ex.Message}\n{ex.StackTrace}",
                    DateTime = DateTime.UtcNow
                });

                context.Response.StatusCode = 500;
                context.Response.Body = originalBodyStream;
                await context.Response.WriteAsJsonAsync(new
                {
                    Exito = false,
                    Mensaje = "Error interno del servidor",
                    Detalle = ex.Message,
                    Timestamp = DateTime.UtcNow
                });
            }
        }
    }
}
