using APICTL.Models;
using APICTL.Utils;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace APICTL.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthenticationController : ControllerBase
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

        [HttpPost]
        public IActionResult ConnectionToDatabse(Credentials credentials)
        {
            User supposedUser;
            string databaseName = ConfigReader.Get("Database");
            MongoClient client = new MongoClient($"mongodb+srv://BDDReader:5cWvQ32s5szGdckL@cluster0.wmqs6p0.mongodb.net/{databaseName}?retryWrites=true&w=majority");
            var collect = client.GetDatabase(databaseName).GetCollection<User>("Users");
            FilterDefinition<User> fd = Builders<User>.Filter.Eq("user", credentials.Login);
            supposedUser = collect.Find(fd).FirstOrDefault();
            if (supposedUser == null)
            {
                return BadRequest();
            }
            var hashedPw = HashTool.HashPassword(credentials.Password, Convert.FromBase64String(supposedUser.PasswordSalt));
            if (supposedUser.Password != hashedPw)
            {
                return BadRequest();
            }
            return Ok(supposedUser);
        }
    }
}
