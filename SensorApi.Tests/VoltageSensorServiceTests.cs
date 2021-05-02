using System;
using System.Linq;

using MongoDB.Driver;
using NUnit.Framework;

using SensorApi.Data.Models;
using SensorApi.Services;

namespace SensorApi.Tests
{
    public class VoltageSensorServiceTests : IDisposable
    {
        DatabaseSettings dbSettings;
        MongoClient client;
        IMongoCollection<VoltageSensorEntry> _collection;

        [SetUp]
        public void Setup()
        {
            dbSettings = new DatabaseSettings
            {
                ConnectionString = "mongodb://localhost:27017",
                DatabaseName = "SensorApiTestDB"
            };

            client = new MongoClient(dbSettings.ConnectionString);
            var database = client.GetDatabase(dbSettings.DatabaseName);
            _collection = database.GetCollection<VoltageSensorEntry>("voltage");
        }

        [Test]
        public void Add_ErrorIsSmallerThanZero_ThrowException()
        {
            _collection.DeleteMany(_ => true);

            var service = new VoltageSensorService(dbSettings);

            Assert.Throws<ArgumentException>(() => service.Add(1.0, -1.0));
        }

        [Test]
        public void Add_AddEntryToDatabase_CollectionIncrease()
        {
            _collection.DeleteMany(_ => true);

            var service = new VoltageSensorService(dbSettings);

            service.Add(1.0, 1.0);

            Assert.That(_collection.CountDocuments(x => true) == 1);
        }

        [Test]
        public void GetLatest_EmptyCollection_ReturnNull()
        {
            _collection.DeleteMany(_ => true);
            var service = new VoltageSensorService(dbSettings);

            Assert.That(service.GetLatest() == null);
        }

        [Test]
        public void GetLatest_ManyEntries_ReturnLastest()
        {
            _collection.DeleteMany(_ => true);

            var service = new VoltageSensorService(dbSettings);
            service.Add(1, 5);
            service.Add(2, 4);
            service.Add(3, 3);
            service.Add(4, 2);
            service.Add(5, 1);

            var latest = service.GetLatest();
            Assert.That(latest.CurrentVoltage == 5 && latest.Error == 1);
        }

        [Test]
        public void List_EmptyCollection_ReturnEmptyList()
        {
            _collection.DeleteMany(_ => true);
            var service = new VoltageSensorService(dbSettings);

            Assert.That(service.List().Count() == 0);
        }

        [Test]
        public void List_FiveElementsInCollection_ReturnListWithFiveElements()
        {
            _collection.DeleteMany(_ => true);
            var service = new VoltageSensorService(dbSettings);
            service.Add(1, 5);
            service.Add(2, 4);
            service.Add(3, 3);
            service.Add(4, 2);
            service.Add(5, 1);

            Assert.That(service.List().Count() == 5);
        }


        public void Dispose()
        {
            client.DropDatabase(dbSettings.DatabaseName);
        }
    }
}