using System;
using System.Collections.Generic;
using System.Linq;
using SensorApi.Data.Models;
using SensorApi.Utils;

namespace SensorApi.Services
{
    class LinearVoltagePredictService : IVoltagePredictService
    {
        public double Predict(TimeSpan time, IEnumerable<VoltageSensorValue> data)
        {
            int count = data.Count();
            double[] x = new double[count];
            double[] y = new double[count];

            for (int i = 0; i < count; i++)
            {
                x[i] = data.ElementAt(i).TimeStamp.ToUnixTimeSeconds();
                y[i] = data.ElementAt(i).CurrentVoltage;
            }

            double rSquared, intercept, slope;
            MathUtils.LinearRegression(x, y, out rSquared, out intercept, out slope);

            var last = data.Last();
            var offset = last.TimeStamp + time;

            return (slope * offset.ToUnixTimeSeconds()) + intercept;
        }
    }
}