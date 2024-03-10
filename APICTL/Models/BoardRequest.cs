using System.Text.Json.Serialization;

namespace APICTL.Models
{ 
    /// <summary>
    /// Classe différente de Board car bool[,] n'est pas sérialisable
    /// </summary>
    public class BoardRequest
    {
        public string Board { get; set; }
        public string Profile { get; set; }

        [JsonConstructor]
        public BoardRequest() { }

        public BoardRequest(Board b)
        {
            Board = b.ToOneLine();
            Profile = string.Format("{0}A{1}D", string.Join("", b.SurviveRules), string.Join("", b.BirthRules));
        }
    }
}
