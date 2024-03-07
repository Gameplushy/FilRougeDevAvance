using APICTL.Models;
using APICTL.Utils;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace APICTL.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthenticationController : ControllerBase
    {
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
