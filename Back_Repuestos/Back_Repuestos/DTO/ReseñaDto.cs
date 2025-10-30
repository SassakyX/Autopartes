namespace Back_Repuestos.DTO
{
    public class ReseñaDto
    {
        public int ProductoId { get; set; }
        public int UsuarioId { get; set; }
        public int Estrellas { get; set; }
        public string? Comentario { get; set; }
    }
}
