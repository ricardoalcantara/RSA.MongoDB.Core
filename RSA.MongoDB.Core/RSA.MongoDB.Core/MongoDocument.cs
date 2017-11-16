using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using RSA.MongoDB.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RSA.MongoDB.Core
{
    public abstract class MongoDocument : IMongoDocument
    {
        [BsonId]
        public ObjectId Id { get; set; }
    }
}
