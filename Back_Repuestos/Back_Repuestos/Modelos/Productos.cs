using System.ComponentModel.DataAnnotations;

namespace Back_Repuestos.Modelos
{
    public class Productos
    {
        [Key]
        public int idProducto { get; set;}
        public string Nombre { get; set; } = string.Empty;
        public string Descrpicion { get; set; } = string.Empty;
        public decimal PrecioCompra { get; set; }
        public decimal PrecioVena { get; set; }
        public int stock { get; set; }

        public byte[]? Imagen { get; set; }

        //Relaciones entre tablas
        public int IdCategoria { get; set; }
        public Categoria? Categoria { get; set; } 
        
        public ICollection<DetalleVenta> DetalleVentas { get; set; }
    }
}
