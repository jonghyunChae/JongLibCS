using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Study
{
    public class Graph
    {
        public Graph(string[][] arrs)
        {
            Nodes = new Node[arrs.Length][];
            for (int i = 0; i < arrs.Length; ++i)
            {
                Nodes[i] = new Node[arrs[i].Length];
                for (int j = 0; j < arrs[i].Length; ++j)
                {
                    var node = Node.Parse(arrs[i][j]);
                    if (node == null)
                    {
                        throw new Exception("노드로 변환할 수 없습니다.");
                    }

                    node.X = j;
                    node.Y = i;

                    if (node is CurrentNode)
                    {
                        Current = (CurrentNode)node;
                    }
                    else if (node is GoalNode)
                    {
                        Goal = (GoalNode)node;
                    }
                    else if (node is BlockedNode)
                    {
                        ++BlockNodeCount;
                    }

                    Nodes[node.Y][node.X] = node;
                    ++NodeCount;
                }
            }

            if (Current == null)
            {
                throw new Exception("현재 위치를 알 수 없습니다.");
            }
        }

        public int BlockNodeCount { get; private set; }
        public int NodeCount { get; private set; }
        public List<Node> Visits { get; private set; } = new List<Node>();
        public CurrentNode Current { get; private set; }
        public GoalNode Goal { get; private set; }
        public Node[][] Nodes { get; private set; }

        readonly VisitedNode visited = new VisitedNode();

        private void Visit(Node node)
        {
            Visits.Add(node);
            Nodes[Current.Y][Current.X] = visited;
            Nodes[node.Y][node.X] = Current;

            Current.X = node.X;
            Current.Y = node.Y;
        }

        public void PrintVisits()
        {
            Console.WriteLine("총 방문 노드 수 : " + Visits.Count);
            var nodes = Visits.Select(node => $"[{node.X}, {node.Y}]");
            Console.WriteLine($"Visited : {string.Join(" ", nodes)}");
        }

        public enum MoveType
        {
            NONE = -1,

            LEFT,
            RIGHT,
            UP,
            DOWN,

            MAX,
        }

        public Node GetNextNode(Node node, MoveType move)
        {
            int next_x = node.X;
            int next_y = node.Y;

            switch (move)
            {
                case MoveType.LEFT:
                    next_x--;
                    if (next_x < 0)
                    {
                        return null;
                    }
                    break;

                case MoveType.RIGHT:
                    next_x++;
                    if (Nodes[next_y].Length <= next_x)
                    {
                        return null;
                    }
                    break;

                case MoveType.UP:
                    next_y--;
                    if (next_y < 0)
                    {
                        return null;
                    }
                    break;

                case MoveType.DOWN:
                    next_y++;
                    if (Nodes.Length <= next_y)
                    {
                        return null;
                    }
                    break;
            }

            var nextNode = Nodes[next_y][next_x];
            if (nextNode == visited)
            {
                return null;
            }
            if (nextNode is BlockedNode)
            {
                return null;
            }
            return nextNode;
        }

        public void MoveTo(MoveType move)
        {
            var nextNode = GetNextNode(Current, move);
            if (nextNode == null)
            {
                Console.WriteLine("갈 수 없는 곳 입니다.");
                return;
            }

            Visit(nextNode);
        }

        public override string ToString()
        {
            StringBuilder str = new StringBuilder();
            for (int i = 0; i < Nodes.Length; ++i)
            {
                for (int j = 0; j < Nodes[i].Length; ++j)
                {
                    str.Append($"{Nodes[i][j].ToString()}\t");
                }
                str.AppendLine();
            }
            return str.ToString();
        }
    }

}
