namespace comp1640_dotnet.Services.Interfaces
{
	public interface IEmailService
	{
		string SendEmail(string sendToEmail, string subject);
		
	}
}
