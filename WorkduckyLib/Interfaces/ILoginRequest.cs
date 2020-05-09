using WorkduckyLib.Enums;

namespace WorkduckyLib.Interfaces
{
    public interface ILoginRequest
    {
        AuthenticationTypes AuthType { get; set; }
    }
}