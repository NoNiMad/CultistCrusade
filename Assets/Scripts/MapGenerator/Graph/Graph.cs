using System.Collections.Generic;

namespace PrimAlgorithm
{
    public class Graph<T>
    {
        private List<Node> nodes_ = new List<Node>();

        public class Node
        {
            private List<Edge> edges_ = new List<Edge>();
            private T context_;

            public T context
            {
                get { return context_; }
            }

            public List<Edge> edges
            {
                get { return edges_; }
            }

            public Node(T context)
            {
                context_ = context;
            }
        }

        public class Edge
        {
            Node to_;
            int weight_;

            public Node to
            {
                get { return to_; }
            }

            public int weight
            {
                get { return weight_; }
            }

            public Edge(Node to, int weight)
            {
                to_ = to;
                weight_ = weight;
            }
        }

        public int n
        {
            get { return nodes_.Count; }
        }

        public List<Node> nodes
        {
            get { return nodes_; }
        }

        public Graph(IEnumerable<T> initialize_list)
        {
            foreach (var i in initialize_list)
            {
                AddVertex(i);
            }
        }

        public Graph(Graph<T> othr)
        {
            CopyFrom(othr);
        }

        public Node AddVertex(T context)
        {
            Node a = new Node(context);
            nodes_.Add(a);
            return a;
        }

        public Node FindVertex(T context)
        {
            return nodes_.Find(n => n.context.Equals(context));
        }

        public void CopyFrom(Graph<T> othr)
        {
            nodes_.Clear();
            nodes_.AddRange(othr.nodes_);
        }

        public bool Contains(Node node)
        {
            return nodes_.Find(n => n.Equals(node)) != null;
        }

        public bool Contains(T context)
        {
            return FindVertex(context) != null;
        }

        public void Remove(Node node)
        {
            nodes_.Remove(node);
        }
    }
}