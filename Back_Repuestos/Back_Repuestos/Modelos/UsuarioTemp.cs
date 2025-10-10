using System.ComponentModel.DataAnnotations;

namespace Back_Repuestos.Modelos
{
    public class UsuarioTemp
    {

        [Key]
        public int IdTemporal { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre_apellido { get; set; } = string.Empty;

        [Required]
        [StringLength(8)]
        public string DNI { get; set; } = string.Empty;

        public string? Direccion { get; set; }

        [Required]
        public string Correo { get; set; } = string.Empty;

        [Required]
        public string User { get; set; } = string.Empty;

        [Required]
        public string Contrasenia { get; set; } = string.Empty;

        public string Rol { get; set; } = "cliente";

        [Required]
        public string CodigoVerificacion { get; set; } = string.Empty;

        public DateTime FechaExpira { get; set; } = DateTime.Now.AddMinutes(10);
    }
}
