using ApiClientes.Models;
using ApiClientes.Models.DTOs;

namespace ApiClientes.Services
{
    public interface IClienteService
    {
        Task<ApiResponse<ClienteResponseDto>> RegistrarClienteAsync(ClienteRegistroDto dto);
        Task<ApiResponse<List<ArchivoResponseDto>>> SubirArchivosAsync(string ci, IFormFile archivoZip);
        Task<Cliente?> ObtenerClientePorCIAsync(string ci);
        Task<List<Cliente>> ObtenerTodosClientesAsync();
    }
}
