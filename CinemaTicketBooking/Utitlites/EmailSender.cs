using System.Net;
using System.Net.Mail;

namespace CinemaTicketBooking.Utitlites
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("01154317756t@gmail.com", "zpyv yukh crgk zhgr")
            };

            return client.SendMailAsync(
                new MailMessage(from: "01154317756t@gmail.com", to: email, subject, htmlMessage)
                {
                    IsBodyHtml = true
                });
        }
    }
}


