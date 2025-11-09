using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiClientes.Models
{
    [Table("LogsApi")]
    [Index(nameof(DateTime), IsDescending = new[] { true })]
    [Index(nameof(TipoLog))]
    [Index(nameof(CodigoEstado))]
    public class LogApi
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdLog { get; set; }

        [Required]
        public DateTime DateTime { get; set; } = DateTime.UtcNow;

        [Required]
        [StringLength(50)]
        public string TipoLog { get; set; } = null!; // INFO, ERROR, WARNING

        [Column(TypeName = "nvarchar(max)")]
        public string? RequestBody { get; set; }

        [Column(TypeName = "nvarchar(max)")]
        public string? ResponseBody { get; set; }

        [StringLength(500)]
        public string? UrlEndpoint { get; set; }

        [StringLength(10)]
        public string? MetodoHttp { get; set; }

        [StringLength(50)]
        public string? DireccionIp { get; set; }

        [Column(TypeName = "nvarchar(max)")]
        public string? Detalle { get; set; }

        public int? CodigoEstado { get; set; }
    }
}
