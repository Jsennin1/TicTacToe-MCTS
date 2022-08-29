namespace TicTacToe_MCTS
{
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

}