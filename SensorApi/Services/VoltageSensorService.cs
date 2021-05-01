using MongoDB.Driver;
using SensorApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SensorApi.Services
{
    public class VoltageSensorService : IVoltageSensorService
    {
        IMongoCollection<VoltageSensorValue> _collection;

        public VoltageSensorService(DatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _collection = database.GetCollection<VoltageSensorValue>("voltage");
        }

        public void Add(double voltage, double error)
        {
            var value = new VoltageSensorValue
            {
                TimeStamp = DateTime.Now,
                CurrentVoltage = voltage,
                Error = error
            };

            _collection.InsertOne(value);
        }

        public VoltageSensorValue GetLatest()
        {
            return _collection.Find(x => true).ToList().LastOrDefault();
        }

        public IEnumerable<VoltageSensorValue> List()
        {
            return _collection.Find(book => true).ToList();
        }
    }
}
