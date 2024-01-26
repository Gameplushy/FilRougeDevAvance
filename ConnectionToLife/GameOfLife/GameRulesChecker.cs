namespace ConnectionToLife.GameOfLife
{
    public static class GameRulesChecker
    {
        public static void Iterate(Board currentIteration)
        {
            bool[,] newIteration = new bool[Board.BOARDSIZE, Board.BOARDSIZE];
            for (int i = 0; i < Board.BOARDSIZE; i++)
            {
                for (int j = 0; j < Board.BOARDSIZE; j++)
                {
                    int livingNeighbors = GetLivingNeighbors(currentIteration.LiveBoard,i, j);
                    newIteration[i, j] = currentIteration.LiveBoard[i, j] ? 
                        currentIteration.SurviveRules.Contains(livingNeighbors) 
                        : currentIteration.BirthRules.Contains(livingNeighbors);
                }
            }
            currentIteration.LiveBoard = newIteration;
        }

        private static int GetLivingNeighbors(bool[,] board, int i, int j)
        {
            int livingNeighbors = 0;
            for(int iOffset = -1; iOffset <= 1; iOffset++)
            {
                for (int jOffset = -1; jOffset <= 1; jOffset++)
                {
                    if ((iOffset == 0 && jOffset == 0) ||
                        (i + iOffset < 0 || j + jOffset < 0 || i + iOffset >= Board.BOARDSIZE || j + jOffset >= Board.BOARDSIZE)) 
                        continue;
                    if (board[i+iOffset, j+jOffset])
                        livingNeighbors++;

                }
            }
            return livingNeighbors;
        }
    }
}
