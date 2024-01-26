using ConnectionToLife.Connection;
using ConnectionToLife.GameOfLife;

User user = UserConnect.ConnectionPrompt();

var b = new Board(user.rules);
b.GenerateRandomBoard();
Console.WriteLine(b.DisplayBoard());
Console.WriteLine($"Code règles : {user.rules}");
Console.ReadLine();
Console.Clear();
Thread t = new Thread(new ThreadStart(Iterate));
t.Start();
Console.ReadLine();
t.Interrupt();


void Iterate()
{
    for (int i = 0; i < 10000; i++)
    {
        GameRulesChecker.Iterate(b);
        Console.SetCursorPosition(0, 0);
        Console.WriteLine(b.DisplayBoard());
        try
        {
            Thread.Sleep(500);
        }
        catch (ThreadInterruptedException tie) { break; }
    }
}

// Get user input for the password
/*string password = "sosis";

// Generate a random salt
byte[] salt = HashTool.GenerateSalt();

// Hash the password using PBKDF2
string hashedPassword = HashTool.HashPassword(password, salt);

// Display the results
Console.WriteLine($"Original Password: {password}");
Console.WriteLine($"Salt: {Convert.ToBase64String(salt)}");
Console.WriteLine($"{hashedPassword}");*/