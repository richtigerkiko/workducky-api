namespace WorkduckyLib.Interfaces
{
    public interface IEmailPasswordRequest : IUser, IRegistrationRequest
    {
        string Password { get; set; }
    }
}