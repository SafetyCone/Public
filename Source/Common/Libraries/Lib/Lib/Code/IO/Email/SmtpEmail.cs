using System;
using System.Net;
using System.Net.Mail;

using Public.Common.Lib.Security;


namespace Public.Common.Lib.Email
{
    public class SmtpEmail
    {
        #region Static

        public static void Send(string to, string from, string subject, string body, string host, int port, Authentication authentication)
        {
            MailAddress toAddress = new MailAddress(to);
            MailAddress fromAddress = new MailAddress(from);

            SmtpClient smptClient = new SmtpClient
            {
                Host = host,
                Port = port,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = new NetworkCredential(authentication.UserName, authentication.Password),
                Timeout = 20000,
            };

            using (MailMessage message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body,
            })
            {
                smptClient.Send(message);
            }
        }

        #endregion
    }
}
