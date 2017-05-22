using System;

using Public.Common.Lib.Security;


namespace Public.Common.Lib.Email
{
    public class Gmail
    {
        public const string Host = @"smtp.gmail.com";
        public const int Port = 587;


        #region Static

        public static void SendEmail(string to, string from, string subject, string body, Authentication authentication)
        {
            SmtpEmail.Send(to, from, subject, body, Gmail.Host, Gmail.Port, authentication);
        }

        #endregion
    }
}
