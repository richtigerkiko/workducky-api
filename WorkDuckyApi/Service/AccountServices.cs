using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using WorkDuckyApi.Factory;
using WorkduckyLib.Interfaces;
using WorkDuckyApi.Service;
using WorkDuckyAPI.DataAccess.Mongo;
using WorkduckyLib.DataObjects;
using WorkduckyLib.DataObjects.RequestObjects;
using WorkduckyLib.DataObjects.ResponseObjects;
using WorkDuckyAPI.Factory;

namespace WorkDuckyAPI.Service
{
    public class AccountServices : IAccountService
    {

        private readonly IConfiguration configuration;
        private readonly ILogger logger;

        public AccountServices(IConfiguration config, ILogger logger)
        {
            configuration = config;
            this.logger = logger;
        }

        public async Task ActivateUser(string token)
        {
            var db = new AccountDataAccess(configuration, logger);
            var tokenServices = new TokenServices(configuration, logger);
            var email = tokenServices.ValidateMailToken(token);
            await db.SetUserActive(email);
        }

        public async Task<User> GetUser(string uid)
        {
            var db = new AccountDataAccess(configuration, logger);
            var userDocument = await db.GetUserDocumentByUidAsync(uid);
            var user = new User()
            {
                Uid = uid,
                Company = new Company(),
                Settings = userDocument.UserSettings
            };
            return user;
        }

        public async Task<UserLoginResponse> LoginUserAsync(ILoginRequest request)
        {
            try
            {
                var factory = new LoginFactory(configuration, logger);
                return await factory.Login(request);
            }
            catch
            {
                throw new ApplicationException();
            }
        }

        public async Task RegisterUserAsync(IRegistrationRequest user)
        {
            try
            {
                var factory = new RegisterFactory(configuration, logger);
                await factory.RegisterUserAsync(user);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Task SendActivationToken(string email)
        {
            throw new NotImplementedException();
        }
    }
}