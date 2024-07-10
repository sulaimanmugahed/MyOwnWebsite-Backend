namespace MyOwnWebsite.Application.Contracts;

public interface IEmailService
{
    Task SendEmail(string toEmail, string subject,string body);
}