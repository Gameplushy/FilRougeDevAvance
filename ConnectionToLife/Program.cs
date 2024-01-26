// See https://aka.ms/new-console-template for more information
using ConnectionToLife.GameOfLife;

var b = new Board("23A3D");
b.GenerateRandomBoard();
Console.WriteLine(b.DisplayBoard());
Console.ReadLine();
for (int i = 0; i < 10; i++)
{
    GameRulesChecker.Iterate(b);
    Console.Clear();
    Console.WriteLine(b.DisplayBoard());
    Console.Read();
}

