using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AdvertisingService.Models
{
    public class Advertisement
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public string ImageUri { get; set; }
    }
}