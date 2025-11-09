using ApiClientes.Data;
using ApiClientes.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiClientes.Services
{
    public class LogService : ILogService
    {
        private readonly AppDbContext _context;
        private readonly ILogger<LogService> _logger;

        public LogService(AppDbContext context, ILogger<LogService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task RegistrarLogAsync(LogApi log)
        {
            try
            {
                _context.LogsApi.Add(log);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al registrar log");
            }
        }

        public async Task<List<LogApi>> ObtenerLogsAsync(int cantidad = 100, string? tipoLog = null)
        {
            var query = _context.LogsApi.AsQueryable();

            if (!string.IsNullOrEmpty(tipoLog))
            {
                query = query.Where(l => l.TipoLog == tipoLog);
            }

            return await query
                .OrderByDescending(l => l.DateTime)
                .Take(cantidad)
                .ToListAsync();
        }

        public async Task<int> LimpiarLogsAntiguosAsync(int diasAntiguedad = 30)
        {
            var fechaLimite = DateTime.UtcNow.AddDays(-diasAntiguedad);

            var logsAntiguos = await _context.LogsApi
                .Where(l => l.DateTime < fechaLimite)
                .ToListAsync();

            _context.LogsApi.RemoveRange(logsAntiguos);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Se eliminaron {Cantidad} logs antiguos", logsAntiguos.Count);

            return logsAntiguos.Count;
        }

    }
}
