using System.ComponentModel.DataAnnotations;

namespace Back_Repuestos.Modelos
{
    public class Usuario
    {
        [Key]
        public int IdUsuario { get; set; }
        [StringLength(100)]
        public string Nombre_apellido { get; set; } = null!;
        [StringLength(20)]
        public string DNI { get; set; }
        [StringLength(255)]
        public string? Direccion { get; set; }
        [StringLength(100)]
        public string Correo { get; set; } = null!;
        [StringLength(50)]
        public string user { get;set; } = null!;
        [StringLength(100)]
        public string Contrasenia { get; set; } = null!;
        [StringLength(20)]
        public string rol { get; set; } = null!;

        public string? CodigoVerificacion { get; set; }
        public DateTime? CodigoExpira { get; set; }

        public ICollection<Venta> Venta { get; set; }

    }
}
