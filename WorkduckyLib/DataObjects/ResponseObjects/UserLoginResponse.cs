namespace WorkduckyLib.DataObjects.ResponseObjects
{
    public class UserLoginResponse : AbstractResponse
    {
        public Token Token { get; set; }
        public User User { get; set; }
    }
}