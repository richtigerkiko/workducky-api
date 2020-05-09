using System.Threading.Tasks;
using WorkduckyLib.Interfaces;
using WorkduckyLib.DataObjects;
using WorkduckyLib.DataObjects.RequestObjects;
using WorkduckyLib.DataObjects.ResponseObjects;

namespace WorkDuckyApi.Service
{
    public interface IAccountService
    {
        Task<UserLoginResponse> LoginUserAsync(ILoginRequest request);
        Task RegisterUserAsync(IRegistrationRequest request);
        Task ActivateUser(string token);
        Task<User> GetUser(string uid);
        Task SendActivationToken(string email);
    }
}