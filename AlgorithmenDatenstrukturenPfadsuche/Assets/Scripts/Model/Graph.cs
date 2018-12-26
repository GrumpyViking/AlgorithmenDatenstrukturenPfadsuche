using System;
using System.Collections.Generic;

namespace Model
{
	public class Graph
	{
		private IList<Node> nodes;
		private IList<Edge> edges;

		public List<Node> Nodes {
			get {
				return new List<Node> (nodes);
			}
		}
		public List<Edge> Edges {
			get{
				return new List<Edge> (edges);
			}
		}

		public Graph(IList<Node> nodes, IList<Edge> edges)
		{
			this.nodes = new List<Node>(nodes);
			this.edges = new List<Edge>(edges);
		}

		public Graph ()
		{
			nodes = new List<Node>();
			edges = new List<Edge>();
		}

		public void AddNode(Node n)
		{
			nodes.Add (n);
		}

		public void AddEdge(Edge e)
		{
			if (nodes.Contains (e.A) && nodes.Contains (e.B)) {
				edges.Add (e);
				//additional "empty" edge in the other direction 
				Edge residualEdge = new Edge(e.B,e.A,0);
				edges.Add (residualEdge);
			} else {
				throw new ArgumentException ("Edge contains node that is not part of current graph.");
			}
		}

		public Node CreateAndAddNode(float x, float y, String label)
		{
			Node n = new Node (x, y, label);
			AddNode (n);
			return n;
		}

		public Edge CreateAndAddEdge(Node a, Node b, int capacity)
		{
			Edge e= new Edge (a, b, capacity);
			AddEdge(e);
			return e;
		}
	}
}

