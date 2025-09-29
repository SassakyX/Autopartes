using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Back_Repuestos.Services
{
    public interface IEmailService
    {
        Task EnviarCodigoAsync(string destino, string codigo);
    }
    public class ConfCorreo
    {
        public string Host { get; set; } = string.Empty;
        public int Port { get; set; }
        public string User { get; set; } = string.Empty;
        public string Pass { get; set; } = string.Empty;
        public string FromName { get; set; } = "Soporte";
    }
    public class CorreoService : IEmailService
    {
        private readonly ConfCorreo _cfg;

        public CorreoService(IOptions<ConfCorreo> cfg) => _cfg = cfg.Value;

        public async Task EnviarCodigoAsync(string destino, string codigo)
        {   
            var msg = new MimeMessage();
            msg.From.Add(new MailboxAddress(_cfg.FromName, _cfg.User));
            msg.To.Add(MailboxAddress.Parse(destino));
            msg.Subject = "Código de verificación";
            Console.WriteLine($"[CorreoService] Host: {_cfg.Host}, User: {_cfg.User}");
            msg.Body = new TextPart("plain") { Text = $"Tu código de verificación es: {codigo}" };

            using var client = new SmtpClient();
            await client.ConnectAsync(_cfg.Host, _cfg.Port, SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(_cfg.User, _cfg.Pass);
            await client.SendAsync(msg);
            await client.DisconnectAsync(true);
        }
    }
}
