using MongoDB.Driver;
using SensorApi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SensorApi.Services
{
    public class VoltageSensorService : IVoltageSensorService
    {
        IMongoCollection<VoltageSensorEntry> _collection;

        public VoltageSensorService(DatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _collection = database.GetCollection<VoltageSensorEntry>("voltage");
        }

        public void Add(double voltage, double error)
        {
            if (error < 0.0)
            {
                throw new ArgumentException("Error cannot be smaller than 0");
            }

            var value = new VoltageSensorEntry
            {
                TimeStamp = DateTimeOffset.UtcNow,
                CurrentVoltage = voltage,
                Error = error
            };

            _collection.InsertOne(value);
        }

        public VoltageSensorEntry GetLatest()
        {
            return _collection.Find(x => true).ToList().LastOrDefault();
        }

        public IEnumerable<VoltageSensorEntry> List()
        {
            return _collection.Find(book => true).ToList();
        }
    }
}
