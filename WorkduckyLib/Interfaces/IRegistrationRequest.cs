using WorkduckyLib.Enums;

namespace WorkduckyLib.Interfaces
{
    public interface IRegistrationRequest
    {
        AuthenticationTypes AuthType { get; set; }
        string Email { get; set; }
    }
}