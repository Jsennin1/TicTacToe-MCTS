namespace TicTacToe_MCTS
{
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

}