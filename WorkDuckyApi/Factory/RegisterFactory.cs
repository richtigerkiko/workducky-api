using System.Threading.Tasks;
using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using WorkDuckyApi.Actions.Account;
using WorkduckyLib.DataObjects.RequestObjects;
using WorkduckyLib.Interfaces;

namespace WorkDuckyAPI.Factory
{
    public class RegisterFactory
    {
        private readonly IConfiguration config;
        private readonly ILogger logger;

        public RegisterFactory(IConfiguration config, ILogger logger)
        {
            this.config = config;
            this.logger = logger;
        }

        public async Task RegisterUserAsync(IRegistrationRequest request)
        {
            switch (request)
            {
                case UserRegistrationEmailPasswordRequest emailPasswordRequest:
                    var action = new RegisterUserEmailPassword(config,logger);
                    await action.Register(request);
                    break;
                default:
                    throw new NotImplementedException("Authtype Unknown");
            }
        }
    }
}