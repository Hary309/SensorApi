using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SensorApi.Data.Models;
using SensorApi.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SensorApi.Controllers
{
    [ApiController]
    [Route("sensor")]
    public class VoltageSensorController : ControllerBase
    {
        IVoltageSensorRepository voltageRepository;
        public VoltageSensorController(IVoltageSensorRepository repository)
        {
            voltageRepository = repository;
        }

        [HttpGet("recent")]
        public double Recent()
        {
            return voltageRepository.GetLatest().CurrentVoltage;
        }

        [HttpGet("predict")]
        public double Predict()
        {
            return 0;
        }
    }
}
