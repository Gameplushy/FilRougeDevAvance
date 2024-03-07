using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace APICTL.Models
{
    public class User
    {
        [BsonId]
        [BsonElement("_id")]
        ObjectId Id { get; set; }
        [BsonElement("user")]
        public string Username { get; set; }
        [BsonElement("salt")]
        public string PasswordSalt { get; set; }
        [BsonElement("password")]
        public string Password { get; set; }
        [BsonElement("rules")]
        public string Rules { get; set; }
    }
}
