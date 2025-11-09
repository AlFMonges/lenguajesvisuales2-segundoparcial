using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ApiClientes.Models
{
    [Table("Clientes")]
    public class Cliente
    {
        [Key]
        [StringLength(50)]
        public string CI { get; set; } = null!;

        [Required(ErrorMessage = "Los nombres son obligatorios")]
        [StringLength(200)]
        public string Nombres { get; set; } = null!;

        [Required(ErrorMessage = "La dirección es obligatoria")]
        [StringLength(500)]
        public string Direccion { get; set; } = null!;

        [Required(ErrorMessage = "El teléfono es obligatorio")]
        [StringLength(20)]
        public string Telefono { get; set; } = null!;

        [Column(TypeName = "varbinary(max)")]
        public byte[]? FotoCasa1 { get; set; }

        [Column(TypeName = "varbinary(max)")]
        public byte[]? FotoCasa2 { get; set; }

        [Column(TypeName = "varbinary(max)")]
        public byte[]? FotoCasa3 { get; set; }

        [Required]
        public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;

        // Navegación
        public virtual ICollection<ArchivoCliente> Archivos { get; set; } = new List<ArchivoCliente>();
    }
}
