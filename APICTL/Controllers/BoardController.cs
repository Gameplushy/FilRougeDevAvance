using APICTL.Models;
using Microsoft.AspNetCore.Mvc;

namespace APICTL.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BoardController : ControllerBase
    {
        [HttpPost]
        public IActionResult Iterate(BoardRequest currentIteration)
        {
            Board b = new Board(currentIteration.Profile);
            b.FromString(currentIteration.Board);
            string res = "";
            for (int i = 0; i < Board.BOARDSIZE; i++)
            {
                for (int j = 0; j < Board.BOARDSIZE; j++)
                {
                    int livingNeighbors = GetLivingNeighbors(b.LiveBoard, i, j);
                    res += (b.LiveBoard[i, j] ?
                        b.SurviveRules.Contains(livingNeighbors)
                        : b.BirthRules.Contains(livingNeighbors)) ? "X" : "·";
                }
            }
            return Ok(res);
        }

        private static int GetLivingNeighbors(bool[,] board, int i, int j)
        {
            int livingNeighbors = 0;
            for (int iOffset = -1; iOffset <= 1; iOffset++)
            {
                for (int jOffset = -1; jOffset <= 1; jOffset++)
                {
                    if ((iOffset == 0 && jOffset == 0) ||
                        (i + iOffset < 0 || j + jOffset < 0 || i + iOffset >= Board.BOARDSIZE || j + jOffset >= Board.BOARDSIZE))
                        continue;
                    if (board[i + iOffset, j + jOffset])
                        livingNeighbors++;

                }
            }
            return livingNeighbors;
        }

    }
}
