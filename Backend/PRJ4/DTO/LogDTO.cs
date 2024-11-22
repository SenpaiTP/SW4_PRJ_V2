using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PRJ4.DTOs
{
    [BsonIgnoreExtraElements]
    public class LogDto
    {
        public ObjectId Id { get; set; }
        public string Level { get; set; } // e.g., Info, Warning, Error
        public string Method {get; set;}
        public DateTime Timestamp { get; set; }
        public string MessageTemplate { get; set; }
        public Dictionary<string, object> Properties { get; set; } = new(); // Stores extra structured data
        public string Exception { get; set; } // For storing exception messages if any
    }
}
