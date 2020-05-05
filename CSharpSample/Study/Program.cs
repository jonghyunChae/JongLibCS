using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
namespace Study
{
    class Program
    {
        static Graph graph;
        static void Main(string[] args)
        {
            var arrs = LoadFile(@"graph_input.txt");
            graph = new Graph(arrs);

            Console.WriteLine(graph);

            BestFistSearch();
            Queue<Graph.MoveType> inputs = BackTracking();
            Simulate(inputs);
        }

        static Queue<Graph.MoveType> Sample()
        {
            var inputs = new Queue<Graph.MoveType>();
            inputs.Enqueue(Graph.MoveType.RIGHT);
            inputs.Enqueue(Graph.MoveType.RIGHT);
            inputs.Enqueue(Graph.MoveType.DOWN);
            inputs.Enqueue(Graph.MoveType.DOWN);
            inputs.Enqueue(Graph.MoveType.DOWN);
            inputs.Enqueue(Graph.MoveType.RIGHT);
            inputs.Enqueue(Graph.MoveType.RIGHT);
            inputs.Enqueue(Graph.MoveType.RIGHT);
            inputs.Enqueue(Graph.MoveType.RIGHT);
            inputs.Enqueue(Graph.MoveType.DOWN);
            return inputs;
        }

        // logic
        static void DepthFirstSearch()
        {
            var closed = new HashSet<Node>();
            var opens = new Stack<Node>();

            opens.Push(graph.Current);

            while (opens.Count != 0)
            {
                Node target = opens.Pop();
                closed.Add(target);

                //Console.WriteLine($"[{target.X}, {target.Y}]");
                if (graph.Goal.X == target.X && graph.Goal.Y == target.Y)
                {
                    break;
                }

                for (int i = 0; i < (int)Graph.MoveType.MAX; ++i)
                {
                    var node = graph.GetNextNode(target, (Graph.MoveType)i);
                    if (node != null
                        && closed.Contains(node) == false
                        && opens.Contains(node) == false)
                    {
                        node.Parent = target;
                        opens.Push(node);
                    }
                }
            }

            Console.WriteLine($"node={graph.NodeCount} blocked={graph.BlockNodeCount} closed={closed.Count} opens={opens.Count}");
        }

        static void BreadthFirstSearch()
        {
            var closed = new HashSet<Node>();
            var opens = new Queue<Node>();

            opens.Enqueue(graph.Current);

            while (opens.Count != 0)
            {
                Node target = opens.Dequeue();
                closed.Add(target);

                //Console.WriteLine($"[{target.X}, {target.Y}]");
                if (graph.Goal.X == target.X && graph.Goal.Y == target.Y)
                {
                    break;
                }

                for (int i = 0; i < (int)Graph.MoveType.MAX; ++i)
                {
                    var node = graph.GetNextNode(target, (Graph.MoveType)i);
                    if (node != null
                        && closed.Contains(node) == false
                        && opens.Contains(node) == false)
                    {
                        node.Parent = target;
                        opens.Enqueue(node);
                    }
                }
            }

            Console.WriteLine($"node={graph.NodeCount} blocked={graph.BlockNodeCount} closed={closed.Count} opens={opens.Count}");
        }

        static void BestFistSearch()
        {
        }

        static Queue<Graph.MoveType> BackTracking()
        {
            var trace = new Stack<Graph.MoveType>();
            Node temp = graph.Goal;
            while (temp != graph.Current)
            {
                Graph.MoveType moveType = temp.Parent.GetDirection(temp);
                trace.Push(moveType);
                temp = temp.Parent;
            }

            var inputs = new Queue<Graph.MoveType>();
            while (trace.Count > 0)
            {
                inputs.Enqueue(trace.Pop());
            }
            return inputs;
        }

        static void Simulate(Queue<Graph.MoveType> inputs)
        {
            Console.WriteLine(graph);
            Console.WriteLine("시작하려면 아무키나 입력하세요.");
            Console.ReadKey();

            while (inputs.Count > 0)
            {
                Console.Clear();
                graph.MoveTo(inputs.Dequeue());
                Console.WriteLine(graph);

                if (graph.Goal.X == graph.Current.X && graph.Goal.Y == graph.Current.Y)
                {
                    Console.WriteLine($"목표에 도달했습니다!!!!. 잔여 카운트 : {inputs.Count}");
                    graph.PrintVisits();
                    break;
                }

                Thread.Sleep(500);
            }
        }

        static string[][] LoadFile(string filePath)
        {
            var lines = File.ReadAllLines(filePath);
            var datas = new List<string>();
            int index = 0;
            string[][] results = new string[lines.Length][];
            foreach (var line in lines)
            {
                datas.Clear();
                datas.AddRange(line.Split());
                results[index] = datas.ToArray();
                ++index;
            }

            return results;
        }
    }
}
