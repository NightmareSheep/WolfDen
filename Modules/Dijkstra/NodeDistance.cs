using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Text;

namespace Dijkstra
{
    public struct NodeDistance<T> : IComparable<NodeDistance<T>> where T : INode<T>
    {
        public NodeDistance(T node, int distance)
        {
            Node = node;
            Distance = distance;
        }

        public T Node;
        public int Distance;

        public int CompareTo(NodeDistance<T> obj) => Distance.CompareTo(obj.Distance);
        public bool Equals(NodeDistance<T> obj) => Distance == obj.Distance;

    }
}
