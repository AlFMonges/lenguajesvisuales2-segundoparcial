using System.ComponentModel.DataAnnotations;

namespace ApiClientes.Models.DTOs
{
    public class ClienteRegistroDto
    {
        [Required(ErrorMessage = "El CI es obligatorio")]
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
        [Phone(ErrorMessage = "Formato de teléfono inválido")]
        public string Telefono { get; set; } = null!;

        public IFormFile? FotoCasa1 { get; set; }
        public IFormFile? FotoCasa2 { get; set; }
        public IFormFile? FotoCasa3 { get; set; }
    }
}
