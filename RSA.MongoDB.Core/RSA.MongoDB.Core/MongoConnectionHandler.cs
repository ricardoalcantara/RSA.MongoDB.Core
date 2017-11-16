using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RSA.MongoDB.Core
{
    public class MongoConnectionHandler<T> where T : IMongoDocument
    {
        public IMongoCollection<T> MongoCollection { get; private set; }
        public MongoConnectionHandler(string mongoDBConnectionString, string databaseName, string collectionName = null)
        {
            if (!mongoDBConnectionString.StartsWith("mongodb://"))
            {
                throw new ArgumentException("mongoDBConnectionString must starts with 'mongodb://'");
            }
            //const string connectionString = "mongodb://localhost";

            //// Get a thread-safe client object by using a connection string
            var mongoClient = new MongoClient(mongoDBConnectionString);

            //// Get a reference to the "retrogamesweb" database object 
            //// from the Mongo server object
            var db = mongoClient.GetDatabase(databaseName);

            //// Get a reference to the collection object from the Mongo database object
            //// The collection name is the type converted to lowercase + "s"
            MongoCollection = db.GetCollection<T>(collectionName ?? (typeof(T).Name.ToLower() + "s"));
        }
    }
}
