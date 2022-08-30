namespace TicTacToe_MCTS
{
    public class Board
    {
        int[][] boardValues;
        public static int DEFAULT_BOARD_SIZE = 3;
        public static int IN_PROGRESS = -1;
        public static int DRAW = 0;
        public static int P1 = 1;
        public static int P2 = 2;
        public Board() {
            boardValues = new int[DEFAULT_BOARD_SIZE][];
            for (int i = 0; i < boardValues.GetLength(0); i++)
            {
                boardValues[i] = new int[DEFAULT_BOARD_SIZE];
            }
        }
        // getters and setters
        public void CopyBoard(Board board) 
        {
            setBoardValues(board.getBoardValues());
        }
        public void performMove(int player, Position p)
        {
            boardValues[p.X][p.Y] = player;
        }
        public int[][] getBoardValues()
        {
            return boardValues;
        }
        public void setBoardValues(int[][] newValue)
        {
            boardValues = newValue.Select(a => a.ToArray()).ToArray();
        }

        public int checkStatus()
        {
            int winner =0;
            //diags check
            if ((boardValues[0][0] == P1 && boardValues[1][1] == P1 && boardValues[2][2] == P1) ||
                        (boardValues[2][0] == P1 && boardValues[1][1] == P1 && boardValues[0][2] == P1))
                winner = P1;
            else if ((boardValues[0][0] == P2 && boardValues[1][1] == P2 && boardValues[2][2] == P2) ||
                        (boardValues[2][0] == P2 && boardValues[1][1] == P2 && boardValues[0][2] == P2))
                winner = P2;
            if(winner != 0) return winner;

            //except diags
            for (int i = 0; i < 3; i++)
            {
                if ((boardValues[i][0] == P1 && boardValues[i][1] == P1 && boardValues[i][2] == P1) ||
                        (boardValues[0][i] == P1 && boardValues[1][i] == P1 && boardValues[2][i] == P1))
                    winner = P1;
                if ((boardValues[i][0] == P2 && boardValues[i][1] == P2 && boardValues[i][2] == P2) ||
                        (boardValues[0][i] == P2 && boardValues[1][i] == P2 && boardValues[2][i] == P2))
                    winner = P2;
            }
            if (getEmptyPositions().Count > 0)
                return IN_PROGRESS;

            return winner;
            /* Evaluate whether the game is won and return winner.
               If it is draw return 0 else return -1*/
        }

        public List<Position> getEmptyPositions()
        {
            int size = this.boardValues.Length;
            List<Position> emptyPositions = new List<Position>();
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (boardValues[i][j] == 0)
                        emptyPositions.Add(new Position(i, j));
                }
            }
            return emptyPositions;
        }
        public void printBoard()
        {
            for (int i = 0; i < DEFAULT_BOARD_SIZE; i++)
            {
                for (int j = 0; j < DEFAULT_BOARD_SIZE; j++)
                {
                    Console.Write("| {0} |", boardValues[i][j]);   
                }
                Console.WriteLine("\n");
            }
            Console.WriteLine("\n");
        }
    }

}