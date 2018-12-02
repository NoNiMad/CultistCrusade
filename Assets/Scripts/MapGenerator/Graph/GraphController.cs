using System.Collections.Generic;
using System.Linq;

namespace PrimAlgorithm.GraphController
{
    public static class Controller
    {
        public static void SetEdge<T>(this Graph<T> self, T a, T b, int weight)
        {
            var a_node = self.FindVertex(a);
            var b_node = self.FindVertex(b);
            a_node.AddEdge(b_node, weight);
            b_node.AddEdge(a_node, weight);
        }

        public static void AddEdge<T>(this Graph<T>.Node self, Graph<T>.Node to, int weight)
        {
            self.edges.Add(new Graph<T>.Edge(to, weight));
        }

        public static List<Graph<T>.Node> GetNeighbors<T>(this Graph<T>.Node self)
        {
            return self.edges.Select(e => e.to).ToList();
        }

        public static int GetWeight<T>(this Graph<T>.Node self, Graph<T>.Node b)
        {
            foreach (var e in self.edges)
            {
                if (e.to == b)
                {
                    return e.weight;
                }
            }

            return int.MaxValue - 1;
        }
    }
}