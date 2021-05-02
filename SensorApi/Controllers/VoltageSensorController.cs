using Microsoft.AspNetCore.Mvc;
using SensorApi.Data.Models;
using SensorApi.Services;
using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SensorApi.Controllers
{
    [ApiController]
    [Route("voltage")]
    public class VoltageSensorController : ControllerBase
    {
        IVoltageSensorService voltageSensorService;

        IVoltagePredictService voltagePredictService;

        public VoltageSensorController(IVoltageSensorService service, IVoltagePredictService predictService)
        {
            voltageSensorService = service;
            voltagePredictService = predictService;
        }

        [HttpGet("recent")]
        public ActionResult<VoltageSensorEntry> Recent()
        {
            return voltageSensorService.GetLatest();
        }

        [HttpGet("predict")]
        public double Predict([FromQuery] int hours = 0, [FromQuery] int minutes = 0, [FromQuery] int seconds = 0)
        {
            TimeSpan timeSpan = new TimeSpan(hours, minutes, seconds);
            var data = voltageSensorService.List();

            return voltagePredictService.Predict(timeSpan, data);
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
