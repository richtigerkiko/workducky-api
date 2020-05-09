using System.Threading.Tasks;
using WorkduckyLib.Interfaces;

namespace WorkDuckyApi.Actions.Account
{
    public interface IRegisterUser
    {
        Task Register(IRegistrationRequest request);
    }
}