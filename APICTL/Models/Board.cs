using System.Text;

namespace APICTL.Models
{
    public class Board
    {
        public const int BOARDSIZE = 10;

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
            SetRules(profile);
        }

        public void SetRules(string profile)
        {
            if (!profile.Contains("A") || !profile.EndsWith("D"))
                throw new FormatException("Should be ##A##D");
            surviveRules = profile.Substring(0, profile.IndexOf("A")).Select(n => int.Parse(n.ToString())).ToList();
            birthRules = profile.Substring(profile.IndexOf("A") + 1, profile.IndexOf("D") - (profile.IndexOf("A") + 1)).Select(n => int.Parse(n.ToString())).ToList();
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
            string s = ToString();
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < BOARDSIZE; i++)
            {
                sb.AppendLine(string.Join("", s.Skip(BOARDSIZE * i).Take(BOARDSIZE)));
            }
            return sb.ToString();
        }

        public byte[] ToBytes()
        {
            List<byte> bytes = new();
            foreach (bool b in board)
            {
                bytes.Add((byte)(b ? 1 : 0));
            }
            return bytes.ToArray();
        }

        public string ToString()
        {
            string res = "";
            foreach (bool b in board)
            {
                res += b ? "X" : "·";
            }
            return res;
        }

        public void FromString(string s)
        {
            for (int i = 0; i < BOARDSIZE; i++)
            {
                for (int j = 0; j < BOARDSIZE; j++)
                {
                    board[i, j] = s[j + i * BOARDSIZE] == 'X';
                }
            }
        }
    }
}
