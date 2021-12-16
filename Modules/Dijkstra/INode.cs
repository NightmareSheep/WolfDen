using System;
using System.Collections.Generic;
using System.Text;

namespace Dijkstra
{
    public interface INode<T> where T : INode<T>
    {
        List<T> Neighbours { get; }
        int Index { get; }
    }
}
