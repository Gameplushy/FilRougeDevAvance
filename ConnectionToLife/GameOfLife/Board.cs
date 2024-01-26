using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectionToLife.GameOfLife
{
    public class Board
    {
        public const int BOARDSIZE = 50;

        bool[,] board;
        List<int> birthRules;
        List<int> surviveRules;

        public bool[,] LiveBoard { get => board; set => board = value; }
        public List<int> BirthRules { get => birthRules; }
        public List<int> SurviveRules { get => surviveRules; }

        public Board(string profile)
        {
            //Format : ##A##D;
            board = new bool[BOARDSIZE, BOARDSIZE];
            if (!profile.Contains("A") || !profile.EndsWith("D"))
                throw new FormatException("Should be ##A##D");
            surviveRules = profile.Substring(0, profile.IndexOf("A")).ToCharArray().Select(n => int.Parse(n.ToString())).ToList();
            birthRules = profile.Substring(profile.IndexOf("A")+1).SkipLast(1).Select(n => int.Parse(n.ToString())).ToList();
        }

        public void GenerateRandomBoard()
        {
            Random rng = new Random();
            for (int i = 0; i < BOARDSIZE; i++)
            {
                for (int j = 0; j < BOARDSIZE; j++)
                {
                    board[i, j] = rng.Next() % 2 == 0;
                }
            }
        }

        public string DisplayBoard()
        {
            StringBuilder sb = new StringBuilder();
            for(int i = 0; i < BOARDSIZE; i++)
            {
                sb.AppendLine(string.Join("",Enumerable.Range(0, BOARDSIZE).Select(e => board[i, e]).Select(b => b ? "*" : ".")));
            }
            return sb.ToString();
        }
    }
}
