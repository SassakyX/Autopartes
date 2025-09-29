using System.ComponentModel.DataAnnotations;

namespace Back_Repuestos.DTO
{
    public class DTOregistro
    {   //Required = campos necesarios y obligatorios 
        [Required(ErrorMessage = "El nombre y apellido es obligatorio")]
        public string Nombre_apellido { get; set; } = string.Empty;

        [Required(ErrorMessage = "El DNI es obligatorio")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "El DNI solo puede contener números")]
        [StringLength(8, MinimumLength = 8, ErrorMessage = "El DNI debe tener 8 dígitos")]
        public string DNI { get; set; } = string.Empty; 

        public string? Direccion { get; set; }

        [Required (ErrorMessage = "El correo es obligatorio")] 
        [EmailAddress(ErrorMessage = "El correo no tiene un formato válido")]
        public string Correo { get; set; } = string.Empty;

        [Required(ErrorMessage = "El nombre de usuario es obligatorio")]
        public string User { get; set; } = string.Empty;


        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [MinLength(5, ErrorMessage = "La contraseña debe tener al menos 5 caracteres")]
        public string Contrasenia { get; set; } = string.Empty;

        public string Rol { get; set; } = "cliente";
    }
}
