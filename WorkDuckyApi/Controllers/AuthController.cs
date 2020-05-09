using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using WorkduckyLib.Interfaces;
using WorkDuckyAPI.Abstract;
using WorkduckyLib.DataObjects;
using WorkduckyLib.DataObjects.RequestObjects;
using WorkduckyLib.DataObjects.ResponseObjects;
using WorkDuckyAPI.Service;

namespace WorkDuckyAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ApiBaseController
    {
        private new ILogger<AuthController> logger;
        private new IConfiguration configuration;

        public AuthController(ILogger<AuthController> logger, IConfiguration configuration) : base(configuration, logger)
        {
            this.logger = logger;
            this.configuration = configuration;
        }

        [HttpPost("login/usernamepassword")]
        public async Task<IActionResult> LoginUsernamePassword(UserLoginEmailPasswordRequest request)
        {
            try
            {
                var accountservice = new AccountServices(configuration, logger);
                var response = await accountservice.LoginUserAsync(request);
                return new JsonResult(response);
            }
            catch (Exception ex)
            {
                logger.LogError("User Login Error", ex);
                return ErrorResult(ex);
            }
        }

        [HttpPost("loginmachine")]
        public IActionResult LoginMachine(UserLoginEmailPasswordMachineRequest request)
        {
            throw new NotImplementedException();
        }
    }
}