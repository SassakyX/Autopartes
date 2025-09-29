using Humanizer.DateTimeHumanizeStrategy;
using System.ComponentModel.DataAnnotations;

namespace Back_Repuestos.Modelos
{
    public class DetalleVenta
    {
        [Key]
        public int IdDventa { get; set; }
        public int Cantidad { get; set; }
        public int Precio_unidad {get; set;}

        public decimal Subtotal { get; set;}    


        public int IdVenta { get; set; }
        public Venta? Venta { get; set; }

        public int IdProducto { get; set; }
        public Productos? Producto { get; set; }

    }
}
