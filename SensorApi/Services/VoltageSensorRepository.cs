using SensorApi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SensorApi.Services
{
    public class VoltageSensorRepository : IVoltageSensorRepository
    {
        public VoltageSensorRepository()
        {

        }

        public void Add(double voltage, double error)
        {
            var value = new VoltageSensor
            {
                Id = Guid.NewGuid().ToString(),
                TimeStamp = DateTime.Now,
                CurrentVoltage = voltage,
                Error = error
            };

        }

        public VoltageSensor GetLatest()
        {
            return null;
        }

        public IEnumerable<VoltageSensor> List()
        {
            return null;
        }
    }
}
