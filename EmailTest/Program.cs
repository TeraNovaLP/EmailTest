using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;
using MailKit.Net.Imap;
using MailKit.Net.Smtp;
using MailKit.Security;
using McMaster.Extensions.CommandLineUtils;

namespace EmailTest
{
    class Program
    {
        static async Task Main(string[] args) => await CommandLineApplication.ExecuteAsync<Program>(args);

        [Required]
        [Option(CommandOptionType.SingleValue)]
        private MailMode Mode { get; }

        [Option(CommandOptionType.NoValue, Description = "Establish a secure connection.")]
        private bool Secure { get; }

        [Option(CommandOptionType.NoValue, Description = "Use STARTTLS to secure the connection.", ShortName = "stls")]
        private bool StartTLS { get; }

        [Option(CommandOptionType.NoValue, Description = "Skip SSL certificate validation.", ShortName = "sv")]
        private bool SkipValidation { get; }

        [Option(CommandOptionType.NoValue)]
        private bool Debug { get; }

        [Required]
        [Argument(0)]
        private string Address { get; }

        [Required]
        [Argument(1)]
        private int Port { get; }

        [Required]
        [Argument(2)]
        private string Username { get; }

        [Required]
        [Argument(3)]
        private string Password { get; }

        private async Task OnExecute(CommandLineApplication app)
        {
            if (Debug)
            {
                foreach (var arg in app.Arguments)
                {
                    Console.WriteLine($"{arg.Name} {arg.Value}");
                }
            }

            if (SkipValidation)
            {
                ServicePointManager.ServerCertificateValidationCallback = (_, _, _, _) => true;
                ServicePointManager.CheckCertificateRevocationList = false;
            }

            try
            {
                if (Mode == MailMode.Imap) await ConnectImap(Address, Port, Username, Password, Secure);
                if (Mode == MailMode.Smtp) await ConnectSmtp(Address, Port, Username, Password, Secure, StartTLS);

                Console.WriteLine("Connection successful!");
            }
            catch (Exception e)
            {
                Console.WriteLine("Connection failed!");
                Console.WriteLine(e);
            }
        }

        private async Task ConnectImap(string address, int port, string username, string password, bool secure)
        {
            using var client = new ImapClient();

            await client.ConnectAsync(address, port, secure);
            await client.AuthenticateAsync(username, password);
            await client.DisconnectAsync(true);
        }

        private async Task ConnectSmtp(string address, int port, string username, string password, bool secure, bool startTls)
        {
            using var client = new SmtpClient();

            await client.ConnectAsync(address, port, secure ? startTls ? SecureSocketOptions.StartTls : SecureSocketOptions.Auto : SecureSocketOptions.None);
            await client.AuthenticateAsync(username, password);
            await client.DisconnectAsync(true);
        }

        private enum MailMode
        {
            Imap,
            Smtp
        }
    }
}
