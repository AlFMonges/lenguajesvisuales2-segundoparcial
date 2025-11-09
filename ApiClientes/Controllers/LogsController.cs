using ApiClientes.Models.DTOs;
using ApiClientes.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApiClientes.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class LogsController : ControllerBase
    {
        private readonly ILogService _logService;

        public LogsController(ILogService logService)
        {
            _logService = logService;
        }

        /// <summary>
        /// Obtiene los últimos logs registrados
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> ObtenerLogs([FromQuery] int cantidad = 100, [FromQuery] string? tipoLog = null)
        {
            var logs = await _logService.ObtenerLogsAsync(cantidad, tipoLog);

            return Ok(new ApiResponse<object>
            {
                Exito = true,
                Mensaje = $"{logs.Count} registros encontrados",
                Datos = logs
            });
        }

        /// <summary>
        /// Elimina logs antiguos
        /// </summary>
        [HttpDelete("limpiar")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> LimpiarLogsAntiguos([FromQuery] int diasAntiguedad = 30)
        {
            var cantidad = await _logService.LimpiarLogsAntiguosAsync(diasAntiguedad);

            return Ok(new ApiResponse<object>
            {
                Exito = true,
                Mensaje = $"Se eliminaron {cantidad} logs antiguos",
                Datos = new { RegistrosEliminados = cantidad }
            });
        }
    }
}
