using System.Diagnostics;

namespace TicTacToe_MCTS
{
    public class Node
    {
        State state;
        Node parent;
        List<Node> childArray;
        public Node()
        {
            state = new State();
            childArray = new List<Node>();
        }
        public void setState(State st)
        {
            state = st;
        }
        public State getState()
        {
            return state;
        }
        public void setParent(Node pt)
        {
            parent = pt;
        }
        public Node getParent()
        {
            return parent;
        }
        public void setChildArray(List<Node> ch)
        {
            childArray = ch;
        }
        public List<Node> getChildArray()
        {
            return childArray;
        }

        public Node getRandomChildNode()
        {
            Random rnd = new Random();
            int randomChildNum = rnd.Next(childArray.Count);
            return childArray[randomChildNum];
        }
        public Node getChildWithMaxScore()
        {
            if (childArray.Count == 0)
                return null;
            double max = int.MinValue;
            Node selectedNode = null;
            foreach (var item in childArray)
            {
                if (item.getState().getWinScore() >= max) {
                    max = item.getState().getWinScore();
                    selectedNode = item;
                }
            }
            return selectedNode;
        }
    }
    public class Tree
    {
        Node root;
        public Tree()
        {
            root = new Node();
        }
        public void setRoot(Node rt)
        {
            root = rt;
        }
        public Node getRoot()
        {
            return root;
        }
    }
    public class State
    {
        Board board;
        int playerNo;
        int visitCount;
        double winScore = 0;
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
                state.playerNo = 3 - playerNo;
                state.board = new Board();
                state.board.setBoardValues(board.getBoardValues());
                state.board.performMove(playerNo, emptyPos[i]);
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
    public class MonteCarloTreeSearch
    {
        static int WIN_SCORE = 10;
        int level;
        int opponent;

        public Board findNextMove(Board board, int playerNo, int endTime)
        {
            // define an end time which will act as a terminating condition

            opponent = 3 - playerNo;
            Tree tree = new Tree();
            Node rootNode = tree.getRoot();
            rootNode.getState().setBoard(board);
            rootNode.getState().setPlayerNo(opponent);

            Stopwatch timer = new Stopwatch();
            timer.Start();
            while (timer.Elapsed.TotalSeconds < endTime)
            {
                Node promisingNode = selectPromisingNode(rootNode);
                if (promisingNode.getState().getBoard().checkStatus()
                  == Board.IN_PROGRESS)
                {
                    expandNode(promisingNode);
                }
                Node nodeToExplore = promisingNode;
                if (promisingNode.getChildArray().Count > 0)
                {
                    nodeToExplore = promisingNode.getRandomChildNode();
                }
                int playoutResult = simulateRandomPlayout(nodeToExplore);
                backPropogation(nodeToExplore, playoutResult);
            }
            timer.Stop();

            Node winnerNode = rootNode.getChildWithMaxScore();
            tree.setRoot(winnerNode);
            return winnerNode.getState().getBoard();
        }
        private Node selectPromisingNode(Node rootNode)
        {
            Node node = rootNode;
            while (node.getChildArray().Count != 0)
            {
                node = UCT.findBestNodeWithUCT(node);
            }
            return node;
        }
        private void expandNode(Node node)
        {
            List<State> possibleStates = node.getState().getAllPossibleStates();
            foreach (var state in possibleStates)
            {
                Node newNode = new Node();
                newNode.setState(state);
                newNode.setParent(node);
                newNode.getState().setPlayerNo(node.getState().getOpponent());
                node.getChildArray().Add(newNode);
            }
        }
        private void backPropogation(Node nodeToExplore, int playerNo)
        {
            Node tempNode = nodeToExplore;
            while (tempNode != null)
            {
                tempNode.getState().incrementVisit();
                if (tempNode.getState().getPlayerNo() == playerNo)
                {
                    tempNode.getState().addScore(WIN_SCORE);
                }
                tempNode = tempNode.getParent();
            }
        }
        private int simulateRandomPlayout(Node node)
        {
            Node tempNode = node;
            State tempState = tempNode.getState();
            int boardStatus = tempState.getBoard().checkStatus();
            if (boardStatus == opponent)
            {
                tempNode.getParent().getState().setWinScore(int.MinValue);
                return boardStatus;
            }
            while (boardStatus == Board.IN_PROGRESS)
            {
                tempState.togglePlayer();
                tempState.randomPlay();
                boardStatus = tempState.getBoard().checkStatus();
            }
            return boardStatus;
        }
    }
    public class UCT
    {
        public static double uctValue(
          int totalVisit, double nodeWinScore, int nodeVisit)
        {
            if (nodeVisit == 0)
            {
                return int.MaxValue;
            }
            return ((double)nodeWinScore / (double)nodeVisit)
              + 1.41 * Math.Sqrt(Math.Log(totalVisit) / (double)nodeVisit);
        }

        public static Node findBestNodeWithUCT(Node node)
        {
            if (node.getChildArray().Count == 0)
                return null;

            int parentVisit = node.getState().getVisitCount();
            double max = int.MinValue;
            Node selectedNode = null;
            foreach (var item in node.getChildArray())
            {
                if (uctValue(parentVisit,item.getState().getWinScore(), item.getState().getVisitCount()) >= max)
                {
                    max = item.getState().getWinScore();
                    selectedNode = item;
                }
            }
            return selectedNode;
        }        
    }
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
            boardValues = newValue;
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
    }
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
            int player = Board.P1;
            int totalMoves = Board.DEFAULT_BOARD_SIZE * Board.DEFAULT_BOARD_SIZE;
            int endTime = 10;
            MonteCarloTreeSearch mcts = new MonteCarloTreeSearch();
            for (int i = 0; i < totalMoves; i++)
            {
                board = mcts.findNextMove(board, player,endTime);
                if (board.checkStatus() != -1)
                {
                    break;
                }
                player = 3 - player;
            }
            int winStatus = board.checkStatus();
            if (winStatus == Board.DRAW)
                Console.WriteLine("draw");
        }
    }
}