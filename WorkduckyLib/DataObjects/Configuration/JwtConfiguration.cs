namespace WorkduckyLib.DataObjects.Configuration
{

    public class JwtConfiguration
    {
        public string Secret { get; set; }
        public int ExpirationTimeInHours { get; set; }
    }

}