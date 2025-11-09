namespace ApiClientes.Models.DTOs
{
    public class ClienteResponseDto
    {
        public string CI { get; set; } = null!;
        public string Nombres { get; set; } = null!;
        public string Direccion { get; set; } = null!;
        public string Telefono { get; set; } = null!;
        public DateTime FechaRegistro { get; set; }
        public int CantidadArchivos { get; set; }
    }
}
