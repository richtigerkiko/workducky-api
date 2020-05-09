using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using WorkduckyLib.Interfaces;
using WorkDuckyAPI.DataAccess.Mongo;
using WorkduckyLib.DataObjects;
using WorkduckyLib.DataObjects.dbo.Mongo;
using WorkduckyLib.DataObjects.RequestObjects;
using WorkduckyLib.DataObjects.ResponseObjects;
using WorkDuckyAPI.Service;

namespace WorkDuckyApi.Actions.Account
{
    public class LoginUserEmailPassword
    {
        private readonly IConfiguration config;
        private readonly ILogger logger;
        public LoginUserEmailPassword(IConfiguration config, ILogger logger)
        {
            this.config = config;
            this.logger = logger;
        }

        public async Task<UserLoginResponse> LoginAsync(ILoginRequest request)
        {
            var user = (UserLoginEmailPasswordRequest)request;
            var db = new AccountDataAccess(config, logger);

            var userDocument = await db.GetDBUserByEmail(user.Email);
            var authenticationDocument = userDocument.AuthenticationDocuments.Where(x => x.Username == user.Email)
                                                                             .FirstOrDefault();

            if (!isPasswordCorrect(user.Password, authenticationDocument.Password))
            {
                throw new Exception("Password incorrect");
            }

            if (!authenticationDocument.isActive)
            {
                throw new Exception("User not activated");
            }

            var tokenServices = new TokenServices(config, logger);

            var token = tokenServices.GenerateToken(userDocument.Uid);

            var response = new UserLoginResponse()
            {
                Token = token,
                User = ConvertMongoUserToUser(userDocument)
            };

            return response;
        }
        
        private bool isPasswordCorrect(string cleartextPassword, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(cleartextPassword, hashedPassword);
        }

        // MACH DAS BESSER
        private User ConvertMongoUserToUser(UserDocument mongoUser)
        {
            var user = new User(){
                Settings = mongoUser.UserSettings,
                Uid = mongoUser.Uid
            };
            return user;
        }
    }
}