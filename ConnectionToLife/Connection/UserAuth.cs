using ConnectionToLife.Utils;
using MongoDB.Driver;

namespace ConnectionToLife.Connection
{
    public class UserAuth
    {
        public static User ConnectionPrompt()
        {
            do
            {
                Console.Write("Insérer login : ");
                string login = Console.ReadLine();
                Console.Write("Insérer mdp : ");
                string password = "";
                while (true)
                {
                    var key = System.Console.ReadKey(true);
                    if (key.Key == ConsoleKey.Enter)
                        break;
                    password += key.KeyChar;
                }
                Console.WriteLine();
                var res = ConnectionToDatabse(login, password);
                if (!string.IsNullOrEmpty(res.error))
                {
                    Console.WriteLine(res.error);
                }
                else
                {
                    Console.WriteLine($"Bonjour {res.res!.Username}!");
                    return res.res;
                }
            }
            while (true);

        }

        public static (User? res,string? error) ConnectionToDatabse(string login, string password)
        {
            User supposedUser;
            MongoClient client = new MongoClient("mongodb+srv://BDDReader:5cWvQ32s5szGdckL@cluster0.wmqs6p0.mongodb.net/COL?retryWrites=true&w=majority");
            var collect = client.GetDatabase("COL").GetCollection<User>("Users");
            FilterDefinition<User> fd = Builders<User>.Filter.Eq("user", login);
            supposedUser = collect.Find(fd).FirstOrDefault();
            if (supposedUser == null)
            {
                return (null, "Login incorrect.");
            }
            var hashedPw = HashTool.HashPassword(password, Convert.FromBase64String(supposedUser.PasswordSalt));
            if (supposedUser.Password != hashedPw)
            {
                return (null, "Mot de passe incorrect.");
            }
            return (supposedUser, null);
        }
    }
}
