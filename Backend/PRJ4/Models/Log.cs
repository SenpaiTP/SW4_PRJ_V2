using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PRJ4.Models
{
    [BsonIgnoreExtraElements]
    public class Log
    {
        public ObjectId Id { get; set; }
        
        public string Level { get; set; } // e.g., Info, Warning, Error
        public DateTime Timestamp { get; set; }
        public string MessageTemplate { get; set; }
        [BsonElement("Properties")]
        public Dictionary<string, object> Properties { get; set; } = new(); // Stores extra structured data
        [BsonElement("Exception")]
        public string Exception { get; set; } // For storing exception messages if any
    }
}
