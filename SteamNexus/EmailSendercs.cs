using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net;
using System.Net.Mail;

namespace SteamNexus
{
    public class EmailSendercs : IEmailSender
    {
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var mail = new MailMessage();
            mail.From = new MailAddress("sr18893828@outlook.com");
            mail.To.Add(email);
            mail.Subject = subject;
            mail.IsBodyHtml = true;
            mail.Body = htmlMessage;
            SmtpClient client = new SmtpClient("smtp-mail.outlook.com");
            client.Port = 587;
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential("sr18893828@outlook.com", "urc58947854k");
            client.EnableSsl = true;
            client.Send(mail);
        }
    }
}
