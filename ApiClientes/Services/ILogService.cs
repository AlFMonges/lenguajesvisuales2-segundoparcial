using ApiClientes.Models;

namespace ApiClientes.Services
{
    public interface ILogService
    {
        Task RegistrarLogAsync(LogApi log);
        Task<List<LogApi>> ObtenerLogsAsync(int cantidad = 100, string? tipoLog = null);
        Task<int> LimpiarLogsAntiguosAsync(int diasAntiguedad = 30);
    }
}
