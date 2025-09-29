namespace Back_Repuestos.DTO
{
    public class LoginDTO
    {
        public string User { get; set; } = string.Empty;
        public string Contrasenia { get; set; } = string.Empty;
    }

    public class VerificarCodigoDTO
    {
        public string User { get; set; } = string.Empty;
        public string Codigo { get; set; } = string.Empty;
    }
}
