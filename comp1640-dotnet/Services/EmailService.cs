using comp1640_dotnet.Services.Interfaces;
using MailKit.Security;
using MimeKit.Text;
using MimeKit;
using MailKit.Net.Smtp;

namespace comp1640_dotnet.Services
{
	public class EmailService: IEmailService
	{
		private readonly IConfiguration configuration;

		public EmailService(IConfiguration _configuration)
		{
			configuration = _configuration;
		}

		public void SendEmail(string sendToEmail, string subject)
		{
			var email = new MimeMessage();
			email.From.Add(MailboxAddress.Parse(configuration["MAILKIT:EMAIL"]));
			email.To.Add(MailboxAddress.Parse(sendToEmail));

			email.Subject = subject;
			email.Body = new TextPart(TextFormat.Html) { Text = "<a href=''> Click </a>" };

			var smtp = new SmtpClient();
			smtp.Connect(configuration["MAILKIT:HOST"], 587, SecureSocketOptions.StartTls);
			smtp.Authenticate(configuration["MAILKIT:EMAIL"], configuration["MAILKIT:PASSWORD"]);

			smtp.Send(email);
			smtp.Disconnect(true);
		}
	}
}
