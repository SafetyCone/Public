using System;


namespace Public.Common.Lib.Security
{
    public class Authentication
    {
        public const string MinexGmailAuthenticationName = @"MinexGmail";


        public string Name { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }


        public Authentication()
        {
        }

        public Authentication(string userName, string password)
        {
            this.UserName = userName;
            this.Password = password;
        }

        public Authentication(string name, string userName, string password)
            : this(userName, password)
        {
            this.Name = name;
        }
    }
}
