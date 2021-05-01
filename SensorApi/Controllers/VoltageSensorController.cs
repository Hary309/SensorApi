using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SensorApi.Models;
using SensorApi.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SensorApi.Controllers
{
    [ApiController]
    [Route("voltage")]
    public class VoltageSensorController : ControllerBase
    {
        IVoltageSensorService voltageSensorService;
        public VoltageSensorController(IVoltageSensorService service)
        {
            voltageSensorService = service;
        }

        [HttpGet("recent")]
        public ActionResult<VoltageSensorValue> Recent()
        {
            return voltageSensorService.GetLatest();
        }

        [HttpGet("predict")]
        public double Predict()
        {
            return 0;
        }

        [HttpGet("feed/{count}")]
        public ActionResult Feed(int count)
        {
            var random = new Random();

            double error = random.NextDouble() / 100;

            double currentValue = random.NextDouble() * 1000;

            var latest = voltageSensorService.GetLatest();

            if (latest is not null)
            {
                currentValue = latest.CurrentVoltage;
            }

            for (var i = 0; i < count; i++)
            {
                voltageSensorService.Add(currentValue, error);

                currentValue += (random.NextDouble() - 0.5) * 10.0;
            }

            return Ok();
        }
    }
}
