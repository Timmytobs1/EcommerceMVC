using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using ValeShop.Interface;
using ValeShop.Models;

public class EmailService : IEmailService
{
    private readonly SmtpSettings _smtpSettings;

    public EmailService(IOptions<SmtpSettings> smtpSettings)
    {
        _smtpSettings = smtpSettings.Value;
    }

    public async Task SendEmailAsync(string toEmail, string subject, string message)
    {
        if (string.IsNullOrEmpty(toEmail))
        {
            throw new ArgumentException("Email address cannot be null or empty", nameof(toEmail));
        }

        var mailMessage = new MailMessage
        {
            From = new MailAddress("kaykelvin2024@gmail.com", "tobi"),
            Subject = subject,
            Body = message,
            IsBodyHtml = true,
        };

        mailMessage.To.Add(toEmail);

        using (var smtpClient = new SmtpClient("smtp.gmail.com"))
        {
            smtpClient.Port = 587;
            smtpClient.Credentials = new NetworkCredential("obadaratimothy@gmail.com", "nkholcupfyhfpmda");
            smtpClient.EnableSsl = true;

            await smtpClient.SendMailAsync(mailMessage);
        }
    }
}
