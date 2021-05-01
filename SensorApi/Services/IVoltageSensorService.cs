using SensorApi.Models;
using System.Collections.Generic;

namespace SensorApi.Services
{
    public interface IVoltageSensorService
    {
        void Add(double voltage, double error);
        VoltageSensorValue GetLatest();
        IEnumerable<VoltageSensorValue> List();
    }
}