using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;

using Public.Common.Lib.Security;


namespace Public.Common.Lib.IO.Email
{
    public class SmtpEmail
    {
        #region Static

        public static void Send(string to, string from, string subject, string body, string host, int port, Authentication authentication, IEnumerable<string> attachmentFilePaths)
        {
            MailAddress toAddress = new MailAddress(to);
            MailAddress fromAddress = new MailAddress(from);

            SmtpClient smtp = new SmtpClient
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
                foreach(string attachmentFilePath in attachmentFilePaths)
                {
                    Attachment attachment = new Attachment(attachmentFilePath);
                    message.Attachments.Add(attachment);
                }

                smtp.Send(message);
            }
        }

        public static void Send(string to, string from, string subject, string body, string host, int port, Authentication authentication)
        {
            SmtpEmail.Send(to, from, subject, body, host, port, authentication, new string[] { });
        }

        #endregion
    }
}
