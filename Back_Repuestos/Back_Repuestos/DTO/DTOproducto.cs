namespace Back_Repuestos.DTO
{
    public class DTOproducto
    {
        public int IdProducto { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Descrpicion { get; set; } = string.Empty;
        public decimal PrecioCompra { get; set; }
        public decimal PrecioVena { get; set; }
        public int Stock { get; set; }
        public string? ImagenBase64 { get; set; }
        public string? Categoria { get; set; }
    }
    public class CrearProductoDTO
    {
        public string Nombre { get; set; } = string.Empty;
        public string Descrpicion { get; set; } = string.Empty;
        public decimal PrecioCompra { get; set; }
        public decimal PrecioVena { get; set; }
        public int Stock { get; set; }
        public int IdCategoria { get; set; }
        public IFormFile? Imagen { get; set; }
    }
}
