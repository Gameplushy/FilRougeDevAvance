using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ConnectionToLife.Connection
{
    public class User
    {
        [BsonId]
        ObjectId _id { get; set; }
        public string user { get; set; }
        public string salt { get; set; }
        public string password { get; set; }
        public string rules { get; set; }
    }
}
