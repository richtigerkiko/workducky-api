namespace WorkduckyLib.DataObjects.Configuration
{

    public class SmtpConfiguration
    {
        public string Server { get; set; }
        public int Port { get; set; }
        public bool UseSSL { get; set; }
        public bool Anonymous { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }

}