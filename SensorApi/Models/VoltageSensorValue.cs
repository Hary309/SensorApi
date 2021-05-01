using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SensorApi.Models
{
    public class VoltageSensorValue
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
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
