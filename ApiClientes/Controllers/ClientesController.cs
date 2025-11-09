using Microsoft.AspNetCore.Mvc;
using ApiClientes.Models.DTOs;
using ApiClientes.Services;
namespace ApiClientes.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class ClientesController : ControllerBase
    {
        private readonly IClienteService _clienteService;
        private readonly ILogger<ClientesController> _logger;

        public ClientesController(IClienteService clienteService, ILogger<ClientesController> logger)
        {
            _clienteService = clienteService;
            _logger = logger;
        }

        /// <summary>
        /// Registra un nuevo cliente con sus fotografías
        /// </summary>
        [HttpPost("registrar")]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RegistrarCliente([FromForm] ClienteRegistroDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Exito = false,
                    Mensaje = "Datos inválidos",
                    Errores = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()
                });
            }

            var resultado = await _clienteService.RegistrarClienteAsync(dto);

            if (!resultado.Exito)
                return BadRequest(resultado);

            return Ok(resultado);
        }

        /// <summary>
        /// Sube múltiples archivos en formato ZIP para un cliente
        /// </summary>
        [HttpPost("{ci}/subir-archivos")]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> SubirArchivos(string ci, IFormFile archivo)
        {
            if (archivo == null || archivo.Length == 0)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Exito = false,
                    Mensaje = "Debe proporcionar un archivo ZIP",
                    Errores = new List<string> { "Archivo no válido" }
                });
            }

            var resultado = await _clienteService.SubirArchivosAsync(ci, archivo);

            if (!resultado.Exito)
                return BadRequest(resultado);

            return Ok(resultado);
        }

        /// <summary>
        /// Obtiene la información de un cliente por su CI
        /// </summary>
        [HttpGet("{ci}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ObtenerCliente(string ci)
        {
            var cliente = await _clienteService.ObtenerClientePorCIAsync(ci);

            if (cliente == null)
            {
                return NotFound(new ApiResponse<object>
                {
                    Exito = false,
                    Mensaje = "Cliente no encontrado"
                });
            }

            return Ok(new ApiResponse<object>
            {
                Exito = true,
                Mensaje = "Cliente encontrado",
                Datos = new
                {
                    cliente.CI,
                    cliente.Nombres,
                    cliente.Direccion,
                    cliente.Telefono,
                    cliente.FechaRegistro,
                    CantidadArchivos = cliente.Archivos.Count,
                    Archivos = cliente.Archivos.Select(a => new
                    {
                        a.IdArchivo,
                        a.NombreArchivo,
                        a.UrlArchivo,
                        a.TamanoBytes,
                        a.FechaSubida
                    })
                }
            });
        }

        /// <summary>
        /// Obtiene todos los clientes registrados
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> ObtenerTodosClientes()
        {
            var clientes = await _clienteService.ObtenerTodosClientesAsync();

            return Ok(new ApiResponse<object>
            {
                Exito = true,
                Mensaje = $"{clientes.Count} clientes encontrados",
                Datos = clientes.Select(c => new
                {
                    c.CI,
                    c.Nombres,
                    c.Direccion,
                    c.Telefono,
                    c.FechaRegistro,
                    CantidadArchivos = c.Archivos.Count
                })
            });
        }
    }
}
