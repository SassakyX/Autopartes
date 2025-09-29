using System.ComponentModel.DataAnnotations;

namespace Back_Repuestos.DTO
{
    public class DTOlogin
    {
        [Required(ErrorMessage = "El usuario o correo es obligatorio")]
        public string User { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        public string Contrasenia { get; set; } = string.Empty;
    }
}
