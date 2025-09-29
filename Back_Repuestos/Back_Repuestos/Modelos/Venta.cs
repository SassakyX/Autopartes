using System.ComponentModel.DataAnnotations;

namespace Back_Repuestos.Modelos
{
    public class Venta
    {
        [Key]
        public int IdVenta { get; set; } 

        public DateTime Fecha {  get; set; }
        public int Total { get; set; }

        public int IdUsuario { get; set; }
        public Usuario? Usuario { get; set; }

        public ICollection<DetalleVenta>? DetalleVentas { get; set; }
    }
}
