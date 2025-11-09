namespace ApiClientes.Models.DTOs
{
    public class ArchivoResponseDto
    {
        public int IdArchivo { get; set; }
        public string NombreArchivo { get; set; } = null!;
        public string UrlArchivo { get; set; } = null!;
        public long TamanoBytes { get; set; }
        public string TamanoFormateado => FormatearTamano(TamanoBytes);
        public DateTime FechaSubida { get; set; }

        private string FormatearTamano(long bytes)
        {
            string[] sizes = { "B", "KB", "MB", "GB" };
            double len = bytes;
            int order = 0;
            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len = len / 1024;
            }
            return $"{len:0.##} {sizes[order]}";
        }
    }
}
