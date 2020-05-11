using System.Threading.Tasks;
using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using WorkDuckyApi.Actions.Account;
using WorkduckyLib.Interfaces;
using WorkduckyLib.DataObjects.RequestObjects;
using WorkduckyLib.DataObjects.ResponseObjects;

namespace WorkDuckyApi.Factory
{
    public class UserFactory
    {
        private readonly IConfiguration config;
        private readonly ILogger logger;

        public UserFactory(IConfiguration config, ILogger logger)
        {
            this.config = config;
            this.logger = logger;
        }

        public async Task<UserLoginResponse> LoginAsync(ILoginRequest request)
        {
            switch (request)
            {
                case UserLoginEmailPasswordRequest _:
                    var action = new LoginUserEmailPassword(config, logger);
                    return await action.LoginAsync(request);

                default:
                    throw new NotImplementedException(string.Format("Can't find Type: {0}", request.GetType().ToString()));
            }
        }

        public async Task RegisterAsync(IRegistrationRequest request)
        {
            switch (request)
            {
                case UserRegistrationEmailPasswordRequest _:
                    var action = new RegisterUserEmailPassword(config, logger);
                    await action.Register(request);
                    break;
                default:
                    throw new NotImplementedException("Authtype Unknown");
            }
        }
    }
}