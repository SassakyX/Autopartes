namespace Back_Repuestos.Modelos
{
    public class Resena
    {
        public int Id { get; set; }
        public int ProductoId { get; set; }
        public int UsuarioId { get; set; }
        public int Estrellas { get; set; }
        public string Comentario { get; set; }
        public DateTime Fecha { get; set; }

        public Productos Producto { get; set; }
        public Usuario Usuario { get; set; }
    }
}
