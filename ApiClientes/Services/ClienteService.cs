using Microsoft.EntityFrameworkCore;
using ApiClientes.Data;
using ApiClientes.Models;
using ApiClientes.Models.DTOs;
using System.IO.Compression;
namespace ApiClientes.Services
{
    public class ClienteService : IClienteService
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<ClienteService> _logger;
        private readonly string[] _extensionesPermitidas = { ".jpg", ".jpeg", ".png", ".pdf", ".doc", ".docx", ".mp4", ".avi" };
        private const long TAMANO_MAXIMO_ARCHIVO = 10 * 1024 * 1024; // 10 MB

        public ClienteService(AppDbContext context, IWebHostEnvironment env, ILogger<ClienteService> logger)
        {
            _context = context;
            _env = env;
            _logger = logger;
        }

        public async Task<ApiResponse<ClienteResponseDto>> RegistrarClienteAsync(ClienteRegistroDto dto)
        {
            try
            {
                // Validar si el cliente ya existe
                if (await _context.Clientes.AnyAsync(c => c.CI == dto.CI))
                {
                    return new ApiResponse<ClienteResponseDto>
                    {
                        Exito = false,
                        Mensaje = "Ya existe un cliente con ese CI",
                        Errores = new List<string> { "CI duplicado" }
                    };
                }

                // Validar fotos si se proporcionan
                var erroresValidacion = new List<string>();
                if (dto.FotoCasa1 != null && !ValidarImagen(dto.FotoCasa1))
                    erroresValidacion.Add("FotoCasa1 no es válida");
                if (dto.FotoCasa2 != null && !ValidarImagen(dto.FotoCasa2))
                    erroresValidacion.Add("FotoCasa2 no es válida");
                if (dto.FotoCasa3 != null && !ValidarImagen(dto.FotoCasa3))
                    erroresValidacion.Add("FotoCasa3 no es válida");

                if (erroresValidacion.Any())
                {
                    return new ApiResponse<ClienteResponseDto>
                    {
                        Exito = false,
                        Mensaje = "Errores de validación en las imágenes",
                        Errores = erroresValidacion
                    };
                }

                var cliente = new Cliente
                {
                    CI = dto.CI,
                    Nombres = dto.Nombres,
                    Direccion = dto.Direccion,
                    Telefono = dto.Telefono,
                    FotoCasa1 = await ConvertirABytesAsync(dto.FotoCasa1),
                    FotoCasa2 = await ConvertirABytesAsync(dto.FotoCasa2),
                    FotoCasa3 = await ConvertirABytesAsync(dto.FotoCasa3),
                    FechaRegistro = DateTime.UtcNow
                };

                _context.Clientes.Add(cliente);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Cliente registrado exitosamente: {CI}", cliente.CI);

                return new ApiResponse<ClienteResponseDto>
                {
                    Exito = true,
                    Mensaje = "Cliente registrado exitosamente",
                    Datos = new ClienteResponseDto
                    {
                        CI = cliente.CI,
                        Nombres = cliente.Nombres,
                        Direccion = cliente.Direccion,
                        Telefono = cliente.Telefono,
                        FechaRegistro = cliente.FechaRegistro,
                        CantidadArchivos = 0
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al registrar cliente: {CI}", dto.CI);
                return new ApiResponse<ClienteResponseDto>
                {
                    Exito = false,
                    Mensaje = "Error al registrar el cliente",
                    Errores = new List<string> { ex.Message }
                };
            }
        }
        public async Task<ApiResponse<List<ArchivoResponseDto>>> SubirArchivosAsync(string ci, IFormFile archivoZip)
        {
            var archivosGuardados = new List<ArchivoResponseDto>();

            try
            {
                // Validar cliente
                var cliente = await _context.Clientes.FindAsync(ci);
                if (cliente == null)
                {
                    return new ApiResponse<List<ArchivoResponseDto>>
                    {
                        Exito = false,
                        Mensaje = "Cliente no encontrado",
                        Errores = new List<string> { "CI no existe" }
                    };
                }

                // Validar extensión ZIP
                if (!Path.GetExtension(archivoZip.FileName).Equals(".zip", StringComparison.OrdinalIgnoreCase))
                {
                    return new ApiResponse<List<ArchivoResponseDto>>
                    {
                        Exito = false,
                        Mensaje = "El archivo debe ser un ZIP",
                        Errores = new List<string> { "Formato inválido" }
                    };
                }

                // Crear carpeta para el cliente
                var carpetaCliente = Path.Combine(_env.ContentRootPath, "uploads", ci);
                Directory.CreateDirectory(carpetaCliente);

                // Descomprimir y guardar archivos
                using (var stream = archivoZip.OpenReadStream())
                using (var zip = new ZipArchive(stream, ZipArchiveMode.Read))
                {
                    foreach (var entry in zip.Entries)
                    {
                        if (string.IsNullOrEmpty(entry.Name)) continue;

                        // Validar extensión
                        var extension = Path.GetExtension(entry.Name).ToLowerInvariant();
                        if (!_extensionesPermitidas.Contains(extension))
                        {
                            _logger.LogWarning("Archivo rechazado por extensión no permitida: {Nombre}", entry.Name);
                            continue;
                        }

                        // Validar tamaño
                        if (entry.Length > TAMANO_MAXIMO_ARCHIVO)
                        {
                            _logger.LogWarning("Archivo rechazado por tamaño excesivo: {Nombre}", entry.Name);
                            continue;
                        }

                        var nombreArchivo = $"{Guid.NewGuid()}{extension}";
                        var rutaCompleta = Path.Combine(carpetaCliente, nombreArchivo);

                        entry.ExtractToFile(rutaCompleta, true);

                        var archivo = new ArchivoCliente
                        {
                            CICliente = ci,
                            NombreArchivo = entry.Name,
                            UrlArchivo = $"/uploads/{ci}/{nombreArchivo}",
                            TamanoBytes = entry.Length,
                            TipoMime = ObtenerTipoMime(entry.Name),
                            FechaSubida = DateTime.UtcNow
                        };

                        _context.ArchivosCliente.Add(archivo);
                        await _context.SaveChangesAsync();

                        archivosGuardados.Add(new ArchivoResponseDto
                        {
                            IdArchivo = archivo.IdArchivo,
                            NombreArchivo = archivo.NombreArchivo,
                            UrlArchivo = archivo.UrlArchivo,
                            TamanoBytes = archivo.TamanoBytes,
                            FechaSubida = archivo.FechaSubida
                        });
                    }
                }

                _logger.LogInformation("Se subieron {Cantidad} archivos para el cliente {CI}", archivosGuardados.Count, ci);

                return new ApiResponse<List<ArchivoResponseDto>>
                {
                    Exito = true,
                    Mensaje = $"{archivosGuardados.Count} archivos subidos exitosamente",
                    Datos = archivosGuardados
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al subir archivos para cliente: {CI}", ci);
                return new ApiResponse<List<ArchivoResponseDto>>
                {
                    Exito = false,
                    Mensaje = "Error al procesar los archivos",
                    Errores = new List<string> { ex.Message }
                };
            }
        }

        public async Task<Cliente?> ObtenerClientePorCIAsync(string ci)
        {
            return await _context.Clientes
                .Include(c => c.Archivos)
                .FirstOrDefaultAsync(c => c.CI == ci);
        }

        public async Task<List<Cliente>> ObtenerTodosClientesAsync()
        {
            return await _context.Clientes
                .Include(c => c.Archivos)
                .OrderByDescending(c => c.FechaRegistro)
                .ToListAsync();
        }

        private async Task<byte[]?> ConvertirABytesAsync(IFormFile? archivo)
        {
            if (archivo == null || archivo.Length == 0) return null;

            using var memoryStream = new MemoryStream();
            await archivo.CopyToAsync(memoryStream);
            return memoryStream.ToArray();
        }

        private bool ValidarImagen(IFormFile archivo)
        {
            if (archivo == null || archivo.Length == 0) return false;

            var extension = Path.GetExtension(archivo.FileName).ToLowerInvariant();
            var extensionesImagen = new[] { ".jpg", ".jpeg", ".png", ".gif" };

            return extensionesImagen.Contains(extension) && archivo.Length <= TAMANO_MAXIMO_ARCHIVO;
        }

        private string ObtenerTipoMime(string nombreArchivo)
        {
            var extension = Path.GetExtension(nombreArchivo).ToLowerInvariant();
            return extension switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                ".pdf" => "application/pdf",
                ".mp4" => "video/mp4",
                ".avi" => "video/x-msvideo",
                ".doc" => "application/msword",
                ".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                _ => "application/octet-stream"
            };
        }
    }
}
