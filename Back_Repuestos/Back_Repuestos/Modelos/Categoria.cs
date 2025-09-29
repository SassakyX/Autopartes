using System.ComponentModel.DataAnnotations;

namespace Back_Repuestos.Modelos
{
    public class Categoria
    {
        [Key]
        public int IdCategoria { get; set; }    
        public string Nombre { get; set; }  
        public string? Desctripcion { get; set; } 
        

        public ICollection<Productos>? Productos { get; set; }
    }
}
