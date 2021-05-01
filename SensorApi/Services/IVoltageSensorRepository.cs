using SensorApi.Data.Models;
using System.Collections.Generic;

namespace SensorApi.Services
{
    public interface IVoltageSensorRepository
    {
        void Add(double voltage, double error);
        VoltageSensor GetLatest();
        IEnumerable<VoltageSensor> List();
    }
}