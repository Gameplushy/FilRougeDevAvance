// See https://aka.ms/new-console-template for more information
using ConnectionToLife.GameOfLife;

var b = new Board("23A3D");
b.GenerateRandomBoard();
Console.WriteLine(b.DisplayBoard());
Console.ReadLine();
Thread t = new Thread(new ThreadStart(Iterate));
t.Start();
Console.ReadLine();
t.Interrupt();


void Iterate()
{
    for (int i = 0; i < 10000; i++)
    {
        GameRulesChecker.Iterate(b);
        Console.Clear();
        Console.WriteLine(b.DisplayBoard());
        try
        {
            Thread.Sleep(500);
        }
        catch(ThreadInterruptedException tie) { break; }
    }
}