using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using WorkduckyLib.DataObjects;
using WorkDuckyAPI.Service;

namespace WorkDuckyAPI.Abstract
{
    public class ApiBaseController : ControllerBase
    {
        private protected ILogger logger;
        private protected IConfiguration configuration;

        public ApiBaseController(IConfiguration configuration, ILogger logger)
        {
            this.configuration = configuration;
            this.logger = logger;
        }

        public async Task<User> GetUser()
        {
            var jwt = Request.Headers["Authorization"].ToString();
            var tokenservices = new TokenServices(configuration, logger);
            return await tokenservices.DecodeJwt(jwt);
        }

        public User Authenticate()
        {
            var user = GetUser().Result;
            if (user == null)
            {
                throw new Exception("Not Authenticated");
            }
            else
            {
                return user;
            }
        }

        public IActionResult ErrorResult(Exception error)
        {
            if(error.Source == "JWT"){
                return Unauthorized("Not Authenticated"); 
            }

            switch (error.Message)
            {
                case "User already exists":
                    return BadRequest(error.Message);
                case "User doesn't exist":
                    return BadRequest("User or Password wrong");
                case "User not found":
                    return Unauthorized("User not found");                    
                case "Password incorrect":
                    return BadRequest("User or Password wrong");
                case "Timer already started":
                    return BadRequest("Timer already started");
                case "token": 
                    return Unauthorized("Not Authenticated");
                case "Not Authenticated":
                    return Unauthorized("Not Authenticated");
                default:
                    logger.LogError(error.Message);
                    return StatusCode(500);
            }
        }
    }
}