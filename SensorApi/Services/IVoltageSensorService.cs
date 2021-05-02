using SensorApi.Data.Models;
using System.Collections.Generic;

namespace SensorApi.Services
{
    public interface IVoltageSensorService
    {
        void Add(double voltage, double error);
        VoltageSensorEntry GetLatest();
        IEnumerable<VoltageSensorEntry> List();
    }
}