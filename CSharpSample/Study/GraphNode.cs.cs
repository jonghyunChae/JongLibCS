using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Study
{
    public class NodeComparer : IComparer<Node>
    {
        public int Compare(Node lhs, Node rhs)
        {
            return rhs.Weight.CompareTo(lhs.Weight);
        }
    }

    public class Node
    {
        public Node Parent { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        public Node(int weight)
        {
            this.Weight = weight;
        }

        public int Weight { get; private set; }
        public override string ToString()
        {
            return this.Weight.ToString();
        }

        public static Node Parse(string str)
        {
            if (int.TryParse(str, out int weight))
            {
                return new Node(weight);
            }

            if (str.Equals(GoalNode.Symbol, StringComparison.OrdinalIgnoreCase))
                return new GoalNode();
            if (str.Equals(CurrentNode.Symbol, StringComparison.OrdinalIgnoreCase))
                return new CurrentNode();
            if (str.Equals(BlockedNode.Symbol, StringComparison.OrdinalIgnoreCase))
                return new BlockedNode();
            return null;
        }

        public bool Contains(Node node)
        {
            Node temp = node.Parent;
            while (temp != null)
            {
                if (temp == node)
                    return true;
                temp = temp.Parent;
            }
            return false;
        }

        public Graph.MoveType GetDirection(Node To)
        {
            if (this.X == To.X && this.Y + 1 == To.Y)
                return Graph.MoveType.DOWN;
            if (this.X == To.X && this.Y - 1 == To.Y)
                return Graph.MoveType.UP;
            if (this.X + 1 == To.X && this.Y == To.Y)
                return Graph.MoveType.RIGHT;
            if (this.X - 1 == To.X && this.Y == To.Y)
                return Graph.MoveType.RIGHT;
            return Graph.MoveType.NONE;
        }
    }

    public class GoalNode : Node
    {
        public GoalNode() : base(int.MaxValue)
        {
        }

        public static string Symbol { get; set; } = "*";

        public override string ToString()
        {
            return Symbol.ToString();
        }
    }

    public class CurrentNode : Node
    {
        public CurrentNode() : base(0)
        {
        }

        public static string Symbol { get; set; } = "@";

        public override string ToString()
        {
            return Symbol.ToString();
        }
    }

    public class VisitedNode : Node
    {
        public VisitedNode() : base(0)
        {
        }

        public static string Symbol { get; set; } = ".";

        public override string ToString()
        {
            return Symbol.ToString();
        }
    }

    public class BlockedNode : Node
    {
        public BlockedNode() : base(-1)
        {
        }

        public static string Symbol { get; set; } = "x";

        public override string ToString()
        {
            return Symbol.ToString();
        }
    }
}
