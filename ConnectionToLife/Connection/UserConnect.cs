using ConnectionToLife.Utils;
using MongoDB.Driver;

namespace ConnectionToLife.Connection
{
    public class UserConnect
    {
        public static User ConnectionPrompt()
        {
            User supposedUser;
            MongoClient client = new MongoClient("mongodb+srv://victorflorent888:yYOzEK0yxEsKmb3c@cluster0.wmqs6p0.mongodb.net/COL?retryWrites=true&w=majority");
            do
            {
                supposedUser = null;
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
                var collect = client.GetDatabase("COL").GetCollection<User>("Users");
                FilterDefinition<User> fd = Builders<User>.Filter.Eq("user", login);
                supposedUser = collect.Find(fd).FirstOrDefault();
                if (supposedUser == null)
                {
                    Console.WriteLine("Login incorrect.");
                    continue;
                }
                var hashedPw = HashTool.HashPassword(password, Convert.FromBase64String(supposedUser.salt));
                if (supposedUser.password != hashedPw)
                {
                    Console.WriteLine("Mot de passe incorrect.");
                    continue;
                }
                break;
                //Check for valid login
                //Encrypt password;
            }
            while (true);
            Console.WriteLine($"Bonjour {supposedUser.user}!");
            return supposedUser;
        }
    }
}
