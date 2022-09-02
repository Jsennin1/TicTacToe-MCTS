namespace TicTacToe_MCTS
{
    public class State
    {
        Board board;
        int playerNo;
        int visitCount;
        double winScore = 0;
        public void CopyState(State state)
        {
            board = new Board();
            board.CopyBoard(state.getBoard());
            playerNo = state.playerNo;
            visitCount = state.visitCount;
            winScore = state.winScore;

        }
        public void setBoard(Board rt)
        {
            board = rt;
        }
        public Board getBoard()
        {
            return board;
        }
        public void setPlayerNo(int plno)
        {
            playerNo = plno;
        }
        public int getPlayerNo()
        {
            return playerNo;
        }
        public void togglePlayer()
        {
            playerNo = 3 - playerNo;
        }
        public void addScore(double score)
        {
            winScore += score;
        }
        public void setWinScore(double score)
        {
            winScore = score;
        }
        public double getWinScore()
        {
            return winScore;
        }

        // copy constructor, getters, and setters

        public List<State> getAllPossibleStates()
        {
            var possibleMoves = new List<State>();
            var emptyPos = board.getEmptyPositions();
            for (int i = 0; i < emptyPos.Count; i++)
            {
                var state = new State();
                state.playerNo = playerNo;
                state.board = new Board();
                state.board.setBoardValues(board.getBoardValues());
                state.board.performMove(3 - playerNo, emptyPos[i]);
                possibleMoves.Add(state);
            }
            return possibleMoves;
            // constructs a list of all possible states from current state
        }
        public void randomPlay()
        {
            /* get a list of all possible positions on the board and 
               play a random move */
            var emptyPos = board.getEmptyPositions();
            Random rnd = new Random();
            int randomChildNum = rnd.Next(emptyPos.Count);
            board.performMove(playerNo,emptyPos[randomChildNum]);
        }
        public int getOpponent()
        {
            return 3 - playerNo;
        }
        public void incrementVisit()
        {
            visitCount++;
        }
        public int getVisitCount()
        {
            return visitCount;
        }
    }

}