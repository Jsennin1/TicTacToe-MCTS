namespace TicTacToe_MCTS
{

    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            givenEmptyBoard_whenSimulateInterAIPlay_thenGameDraw();
        }
        public static void givenEmptyBoard_whenSimulateInterAIPlay_thenGameDraw()
        {
            Board board = new Board();
            int player = Board.P2;
            int totalMoves = Board.DEFAULT_BOARD_SIZE * Board.DEFAULT_BOARD_SIZE;
            int endTime = 1;
            MonteCarloTreeSearch mcts = new MonteCarloTreeSearch();
            for (int i = 0; i < totalMoves; i++)
            {
                board = mcts.findNextMove(board, player,endTime);
                Console.WriteLine("{0}-)", i + 1);
                board.printBoard();

                if (board.checkStatus() != -1)
                {
                    break;
                }
                player = 3 - player;
            }
            int winStatus = board.checkStatus();
                Console.WriteLine("winner " + winStatus);
        }
    }

}