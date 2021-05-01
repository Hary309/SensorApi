using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SensorApi.Data.Models
{
    public class VoltageSensor
    {
        /// <summary>
        /// Given id for the measurement
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Time of measurement
        /// </summary>
        public DateTime TimeStamp { get; set; }

        /// <summary>
        /// Voltage of measurement
        /// </summary>
        public double CurrentVoltage { get; set; }

        /// <summary>
        /// Error of measuring device
        /// </summary>
        public double Error { get; set; }
    }
}
