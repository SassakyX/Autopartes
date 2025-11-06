using System.Net;
using System.Text.Json;

namespace Back_Repuestos.MIDDLEWARE
{
    public class Excepciones
    {

        private readonly RequestDelegate _next;
        private readonly ILogger<Excepciones> _logger;

        public Excepciones(RequestDelegate next, ILogger<Excepciones> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error no controlado");
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";
                var respuesta = JsonSerializer.Serialize(new { mensaje = "Error interno en el servidor." });
                await context.Response.WriteAsync(respuesta);
            }
        }
    }
}
