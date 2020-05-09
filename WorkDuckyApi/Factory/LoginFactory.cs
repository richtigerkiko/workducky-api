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
    public class LoginFactory
    {
        private readonly IConfiguration config;
        private readonly ILogger logger;

        public LoginFactory(IConfiguration config, ILogger logger)
        {
            this.config = config;
            this.logger = logger;
        }

        public async Task<UserLoginResponse> Login(ILoginRequest request)
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
    }
}