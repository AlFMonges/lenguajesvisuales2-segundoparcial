using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiClientes.Models
{
    [Table("ArchivosCliente")]
    [Index(nameof(CICliente))]
    [Index(nameof(FechaSubida))]
    public class ArchivoCliente
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdArchivo { get; set; }

        [Required]
        [StringLength(50)]
        public string CICliente { get; set; } = null!;

        [Required]
        [StringLength(255)]
        public string NombreArchivo { get; set; } = null!;

        [Required]
        [StringLength(1000)]
        public string UrlArchivo { get; set; } = null!;

        [Required]
        public long TamanoBytes { get; set; }

        [StringLength(100)]
        public string? TipoMime { get; set; }

        [Required]
        public DateTime FechaSubida { get; set; } = DateTime.UtcNow;

        // Navegación
        [ForeignKey(nameof(CICliente))]
        public virtual Cliente Cliente { get; set; } = null!;
    }
}
