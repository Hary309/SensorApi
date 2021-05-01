using System;
using System.Collections.Generic;

using SensorApi.Models;

namespace SensorApi.Services
{
    public interface IVoltagePredictService
    {
        double Predict(TimeSpan time, IEnumerable<VoltageSensorValue> data);
    }
}