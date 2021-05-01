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
    }
}
