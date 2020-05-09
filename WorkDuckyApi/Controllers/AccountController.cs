using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using WorkDuckyAPI.Abstract;
using WorkduckyLib.DataObjects.RequestObjects;
using WorkduckyLib.Interfaces;
using WorkDuckyAPI.Service;

namespace WorkDuckyAPI.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class AccountController : ApiBaseController
    {

        private new readonly ILogger<AccountController> logger;
        private new readonly IConfiguration configuration;

        public AccountController(IConfiguration configuration, ILogger<AccountController> logger) : base(configuration, logger)
        {
            this.logger = logger;
            this.configuration = configuration;
        }

        [HttpPost("signup/usernamepassword")]
        public async Task<IActionResult> SignupUsernamePassword(UserRegistrationEmailPasswordRequest userReq)
        {
            try
            {
                var accountServices = new AccountServices(configuration, logger);
                await accountServices.RegisterUserAsync(userReq);

                return new OkResult();
            }
            catch (Exception ex)
            {
                return ErrorResult(ex);
            }
        }

        [HttpPut("ResentToken")]
        public async Task<IActionResult> ResentNewActivationToken(string email)
        {
            var accountService = new AccountServices(configuration, logger);
            await accountService.SendActivationToken(email);
            return new OkResult();
        }

        [HttpGet("Activate")]
        public async Task<IActionResult> ActivateAccountAsync(string token)
        {
            try
            {
                var registrationService = new AccountServices(configuration, logger);
                await registrationService.ActivateUser(token);
                return new OkResult();
            }
            catch (Exception ex)
            {
                return ErrorResult(ex);
            }
        }

        [HttpGet]
        public IActionResult GetAccount()
        {
            try
            {
                return new JsonResult(GetUser());
            }
            catch (Exception ex)
            {
                return ErrorResult(ex);
            }
        }
    }

}