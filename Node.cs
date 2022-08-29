﻿namespace TicTacToe_MCTS
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
            childArray = ch.ToList();
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

}