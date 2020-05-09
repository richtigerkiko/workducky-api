using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using WorkDuckyAPI.Abstract;
using WorkduckyLib.DataObjects.RequestObjects;
using WorkDuckyAPI.Service;
using WorkduckyLib.DataObjects.ResponseObjects;
using Newtonsoft.Json;
using NodaTime.Serialization.JsonNet;
using NodaTime;
using NodaTime.Text;
using System.Collections.Generic;

namespace WorkDuckyAPI.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class TimerController : ApiBaseController
    {

        private new readonly ILogger<TimerController> logger;
        private readonly IConfiguration config;

        public TimerController(IConfiguration configuration, ILogger<TimerController> logger) : base(configuration, logger)
        {
            this.logger = logger;
            this.config = configuration;
        }

        [HttpPost("Start")]
        public IActionResult StartTimer(StartTimerRequest request)
        {
            try
            {
                var user = Authenticate();
                var timerServices = new TimerServices(config, logger);
                var response = timerServices.StartTimer(request, user);
                return new JsonResult(response);
            }
            catch (Exception ex)
            {
                return ErrorResult(ex);
            }
        }

        [HttpPost("Stop")]
        public IActionResult StopTimer(StopTimerRequest request)
        {
            try
            {
                var user = Authenticate();
                var timerServices = new TimerServices(config, logger);
                var response = timerServices.StopTimer(request);
                return new JsonResult(response);
            }
            catch (Exception ex)
            {
                return ErrorResult(ex);
            }
        }

        [HttpPost("Rate")]
        public IActionResult RateTimer(RateTimerRequest request)
        {
            try
            {
                var user = Authenticate();
                var timerServices = new TimerServices(config, logger);
                timerServices.RateTimer(request);
                return Ok();
            }
            catch (Exception ex)
            {
                return ErrorResult(ex);
            }
        }

        [HttpGet("GetActiveTimer")]
        public IActionResult GetActiveTimer()
        {
            try
            {
                var user = Authenticate();
                var timerServices = new TimerServices(config, logger);
                var response = new TimerResponse();
                response = timerServices.GetActiveTimer(user);
                return new JsonResult(response);
            }
            catch (Exception ex)
            {
                return ErrorResult(ex);
            }
        }

        [HttpGet("GetTimer")]
        public IActionResult GetTimer(string id)
        {
            try
            {
                var user = Authenticate();
                var timerServices = new TimerServices(config, logger);
                var response = timerServices.GetTimer(id);
                return new JsonResult(response);
            }
            catch (Exception ex)
            {
                return ErrorResult(ex);
            }
        }

        [HttpPut("StartBreak")]
        public IActionResult StartBreak(BreakRequest request)
        {
            try
            {
                var user = Authenticate();
                var timerServices = new TimerServices(config, logger);
                timerServices.StartBreak(request);
                return Ok();
            }
            catch (Exception ex)
            {
                return ErrorResult(ex);
            }
        }

        [HttpPut("StopBreak")]
        public IActionResult StopBreak(BreakRequest request)
        {
            try
            {
                var user = Authenticate();
                var timerServices = new TimerServices(config, logger);
                timerServices.StopBreak(request);
                return Ok();
            }
            catch (Exception ex)
            {
                return ErrorResult(ex);
            }
        }

        [HttpGet("GetWeekOverview")]
        public IActionResult GetWeekOverview(int year, int week)
        {
            try
            {
                var user = Authenticate();
                var timerServices = new TimerServices(config, logger);
                var response = timerServices.GetWeekOverview(year, week);
                return new JsonResult(response);
            }
            catch (Exception ex)
            {
                return ErrorResult(ex);
            }
        }

        [HttpGet("GetOverTime")]
        public IActionResult GetOverTime()
        {
            try
            {
                var user = Authenticate();
                var timerServices = new TimerServices(config, logger);
                var response = timerServices.CalculateOvertime(user);
                return new JsonResult(response);
            }
            catch (Exception ex)
            {
                return ErrorResult(ex);
            }
        }

        [HttpPut("GenerateDummyData")]
        public IActionResult GenerateDummyData()
        {
            try
            {
                var user = Authenticate();
                var dummyService = new DummyServices(config, logger);
                dummyService.GenerateDummyData(user.Uid);
                return Ok();
            }
            catch (Exception ex)
            {
                return ErrorResult(ex);
            }
        }
    }

}