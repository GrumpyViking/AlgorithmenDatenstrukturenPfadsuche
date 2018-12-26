using System;

namespace Model
{
	public class Edge : IDataObject
	{
		public Node A { get; private set;}
		public Node B { get; private set;}
		public int capacity { get; set;}

		public Edge (Node A, Node B, int capacity)
		{
			this.A = A;
			this.B = B;
			this.capacity = capacity;
		}

		public string GetName()
		{
			return A.Label + "_" + B.Label;
		}
	}
}

