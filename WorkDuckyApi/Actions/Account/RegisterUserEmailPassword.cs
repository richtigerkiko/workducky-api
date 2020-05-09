using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using WorkDuckyAPI.DataAccess.Mongo;
using WorkduckyLib.DataObjects.RequestObjects;
using WorkduckyLib.Interfaces;
using WorkDuckyAPI.Service;

namespace WorkDuckyApi.Actions.Account
{
    public class RegisterUserEmailPassword : IRegisterUser
    {
        private readonly IConfiguration config;
        private readonly ILogger logger;
        public RegisterUserEmailPassword(IConfiguration config, ILogger logger)
        {
            this.config = config;
            this.logger = logger;
        }

        public async Task Register(IRegistrationRequest request)
        {
            var db = new AccountDataAccess(config, logger);
            try
            {
                var userLoginEmailPasswordRequest = (UserRegistrationEmailPasswordRequest)request;

                userLoginEmailPasswordRequest.Password = BCrypt.Net.BCrypt.HashPassword(userLoginEmailPasswordRequest.Password);

                var tokenServices = new TokenServices(config, logger);
                var activationToken = tokenServices.GenerateMailToken(request.Email);

                await db.WriteEmailPasswordUserToDatabaseAsync(userLoginEmailPasswordRequest);

                var mailService = new MailService(config, logger);
                await mailService.SendRegistrationMessage(request.Email, activationToken);
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, 123, ex, "Can't create User");
                throw ex;
            }
        }
    }
}