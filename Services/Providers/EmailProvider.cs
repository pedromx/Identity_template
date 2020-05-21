using DTOs;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Services.Providers
{
    public class EmailProvider : IEmailProvider
    {
        /// <summary>
        /// To use this email service you need to configure your google account to allow the unsecure apps access.  
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <param name="body"></param>
        /// <param name="options"></param>
        public void Send(string emailAddress, string body, EmailProviderSettingsDTO options)
        {
            var client = new SmtpClient();

            client.Host = options.Host;
            client.Port = options.Port;
            client.EnableSsl = true;
            
            var nc = new NetworkCredential(options.ApiKey, options.ApiKeySecret);
            client.UseDefaultCredentials = true;
            client.Credentials = nc;

            var message = new MailMessage(options.SenderEmail, emailAddress);
            message.Body = body;
            message.Subject = "NoReply";

            message.IsBodyHtml = false;

            client.Send(message);
        }
    }
}
