using Core.Domain;
using Core.Domain.Interfaces.Emails;
using Core.Domain.Models.Configurations;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Infrastructure.Email;

public class EmailService(IOptions<EmailConfiguration> configOptions, IAppLogging<EmailService> logger) : IEmailService
{
    private readonly EmailConfiguration emailConfig = configOptions.Value;

    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        var emailMessage = new MimeMessage();

        emailMessage.From.Add(new MailboxAddress(emailConfig.DisplayName, emailConfig.Email));
        emailMessage.To.Add(MailboxAddress.Parse(email));
        emailMessage.Subject = subject;

        var builder = new BodyBuilder { HtmlBody = htmlMessage };
        emailMessage.Body = builder.ToMessageBody();

        using var client = new SmtpClient();
        try
        {
            await client.ConnectAsync(emailConfig.Host, emailConfig.Port, SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(emailConfig.Email, emailConfig.Password);
            await client.SendAsync(emailMessage);
        }
        catch (Exception ex)
        {
            logger.Error("Error sending email", ex);
        }
        finally
        {
            await client.DisconnectAsync(true);
        }
    }
}
