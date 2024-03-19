namespace Core.Domain.Interfaces.Emails;

public interface IEmailService
{
    Task SendEmailAsync(string email, string subject, string htmlMessage);
}
