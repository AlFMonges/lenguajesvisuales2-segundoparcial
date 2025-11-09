namespace ApiClientes.Models.DTOs
{
    public class ApiResponse<T>
    {
        public bool Exito { get; set; }
        public string Mensaje { get; set; } = string.Empty;
        public T? Datos { get; set; }
        public List<string>? Errores { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
