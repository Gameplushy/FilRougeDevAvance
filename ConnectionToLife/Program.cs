// See https://aka.ms/new-console-template for more information
using ConnectionToLife.GameOfLife;

var b = new Board("23A3D");
b.GenerateRandomBoard();
Console.WriteLine(b.DisplayBoard());
