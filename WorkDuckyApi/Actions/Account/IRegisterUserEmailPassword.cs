using System.Threading.Tasks;
using WorkduckyLib.Interfaces;

namespace WorkDuckyApi.Actions.Account
{
    public interface IRegisterUser
    {
        Task RegisterAsync(IRegistrationRequest request);
    }
}