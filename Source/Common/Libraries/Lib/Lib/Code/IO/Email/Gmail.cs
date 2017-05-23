using System;
using System.Collections.Generic;

using Public.Common.Lib.Security;


namespace Public.Common.Lib.IO.Email
{
    public class Gmail
    {
        public const string Host = @"smtp.gmail.com";
        public const int Port = 587;
        public const string AutomationFolderLabel = @"[AUTOMATION]";


        #region Static

        public static void SendEmail(string to, string from, string subject, string body, Authentication authentication, IEnumerable<string> attachments)
        {
            SmtpEmail.Send(to, from, subject, body, Gmail.Host, Gmail.Port, authentication, attachments);
        }

        public static void SendEmail(string to, string from, string subject, string body, Authentication authentication)
        {
            SmtpEmail.Send(to, from, subject, body, Gmail.Host, Gmail.Port, authentication);
        }

        public static string ApplyFolderLabel(string subject, string label)
        {
            string output = String.Format(@"{0} {1}", label, subject);
            return output;
        }

        public static string ApplyAutomationLabel(string subject)
        {
            string output = Gmail.ApplyFolderLabel(subject, Gmail.AutomationFolderLabel);
            return output;
        }

        #endregion
    }
}
