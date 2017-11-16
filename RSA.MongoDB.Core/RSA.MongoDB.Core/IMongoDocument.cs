using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RSA.MongoDB.Core
{
    public interface IMongoDocument
    {
        ObjectId Id { get; set; }
    }
}
