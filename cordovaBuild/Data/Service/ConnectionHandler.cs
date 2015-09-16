using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Core;
using MongoDB.Shared;
using cordovaBuild.Data.Entities.Base;

namespace cordovaBuild.Data.Entities.Service
{
    public class ConnectionHandler<T> where T : IMongoEntity
    {
        public IMongoCollection<T> MongoCollection { get; set; }

        public ConnectionHandler()
        {
            try
            {
                const string connectionString = "mongodb://localhost:27017";

                var mongoClient = new MongoClient(connectionString);

                var db = mongoClient.GetDatabase("cordovaBuild");

                MongoCollection = db.GetCollection<T>(typeof(T).Name.ToLower() + "s");
            }
            catch(Exception ex)
            {
                throw new Exception("Unable to connect to the database. Please contact your administrator.");
            }

        }
    }
}
