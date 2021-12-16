using System;
using System.Collections.Generic;
using C5;

namespace Dijkstra
{
    public static class Dijkstra
    {
        public static void DijkstraAlgorithm<T>(out int[] dist, out int[] prev, T[] nodes, T start, Func<T,T,int> distanceFunction) where T : INode<T>
        {
            dist = new int[nodes.Length];
            prev = new int[nodes.Length];
            var handles = new IPriorityQueueHandle<NodeDistance<T>>[nodes.Length];

            for (int i = 0; i < nodes.Length; i++)
            {
                dist[i] = int.MaxValue;
                prev[i] = -1;
            }
            dist[start.Index] = 0;

            var priorityQueue = new IntervalHeap<NodeDistance<T>>(nodes.Length)
            {
                new NodeDistance<T>(start, 0)
            };

            while (priorityQueue.Count > 0)
            {
                var u = priorityQueue.DeleteMin();

                foreach (var neighbour in u.Node.Neighbours)
                {
                    var distanceToNeighbour = u.Distance + distanceFunction(u.Node, neighbour);
                    if (distanceToNeighbour < dist[neighbour.Index])
                    {
                        dist[neighbour.Index] = distanceToNeighbour;
                        prev[neighbour.Index] = u.Node.Index;
                        //priorityQueue.Replace(handles[neighbour.Index], new NodeDistance<T>(neighbour, distanceToNeighbour));

                        var neighbourHandle = handles[neighbour.Index];
                        if (neighbourHandle != null)
                            priorityQueue.Delete(neighbourHandle);
                        IPriorityQueueHandle<NodeDistance<T>> newHandle = null;
                        priorityQueue.Add(ref newHandle, new NodeDistance<T>(neighbour, distanceToNeighbour));
                        handles[neighbour.Index] = newHandle;
                    }
                }
            }
        }
    }
}
