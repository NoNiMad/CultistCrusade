using System.Collections.Generic;
using PrimAlgorithm.GraphController;

namespace PrimAlgorithm
{
    class Prim<T>
    {
        readonly int kInfinite = int.MaxValue - 1;

        public void prim(Graph<T> g, Graph<T>.Node r, ref Dictionary<Graph<T>.Node, Graph<T>.Node> tree)
        {
            Graph<T> Q = new Graph<T>(g);
            var d = new Dictionary<Graph<T>.Node, int>();
            foreach (var u in Q.nodes)
            {
                d[u] = kInfinite;
            }

            d[r] = 0;

            while (Q.n > 0)
            {
                var u = deleteMin(Q, d);
                foreach (var v in u.GetNeighbors())
                {
                    if (Q.Contains(v) && (u.GetWeight(v) < d[v]))
                    {
                        d[v] = u.GetWeight(v);
                        tree[v] = u;
                    }
                }
            }
        }

        private Graph<T>.Node deleteMin(Graph<T> Q, Dictionary<Graph<T>.Node, int> d)
        {
            Graph<T>.Node min_node = null;
            int min_weight = kInfinite;
            foreach (var n in Q.nodes)
            {
                if (d[n] <= min_weight)
                {
                    min_weight = d[n];
                    min_node = n;
                }
            }

            Q.Remove(min_node);
            return min_node;
        }
    }
}