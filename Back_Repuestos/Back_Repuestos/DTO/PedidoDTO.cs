namespace Back_Repuestos.DTO
{
    public class PedidoDTO
    {
        public int IdVenta { get; set; }
        public DateTime Fecha { get; set; }
        public int Total { get; set; }
        public string Estado { get; set; } = string.Empty;
        public int IdUsuario { get; set; }

        // Info básica del usuario
        public string UsuarioNombre { get; set; } = string.Empty;
        public string UsuarioCorreo { get; set; } = string.Empty;

        public List<DetalleVentaDTO> Detalles { get; set; } = new();
    }

    public class DetalleVentaDTO
    {
        public int IdProducto { get; set; }
        public int Cantidad { get; set; }
        public int PrecioUnidad { get; set; }
        public decimal Subtotal { get; set; }
        public string ProductoNombre { get; set; } = string.Empty;
    }
    public class PedidoCrearDTO
    {
        public int IdUsuario { get; set; }
        public List<PedidoDetalleCrearDTO> Detalles { get; set; } = new();
    }

    public class PedidoDetalleCrearDTO
    {
        public int IdProducto { get; set; }
        public int Cantidad { get; set; }
        public int PrecioUnidad { get; set; }
    }
}
