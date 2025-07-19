using System.Collections.Generic;
using UnityEngine;


public static class LocationPathfinding
{
    public static List<NodeData> Dijkstra(LocationGraph graph, string startId, string goalId, TravelMode travelMode)
    {
        var dist = new Dictionary<string, float>();
        var prev = new Dictionary<string, string>();
        var unvisited = new List<string>();

        foreach (var node in graph.nodes)
        {
            dist[node.nodeId] = float.MaxValue;
            prev[node.nodeId] = null;
            unvisited.Add(node.nodeId);
        }
        dist[startId] = 0f;

        while (unvisited.Count > 0)
        {
            string u = null;
            float minDist = float.MaxValue;
            foreach (var n in unvisited)
            {
                if (dist[n] < minDist)
                {
                    minDist = dist[n];
                    u = n;
                }
            }

            if (u == goalId)
                break;

            unvisited.Remove(u);

            var nodeU = graph.GetNode(u);
            if (nodeU == null) continue;

            foreach (var edge in nodeU.edges)
            {
                if (!unvisited.Contains(edge.toNodeId)) continue;

                float edgeCost = (travelMode == TravelMode.Time)
                                 ? edge.energyCost
                                 : edge.moneyCost;

                float alt = dist[u] + edgeCost;

                if (alt < dist[edge.toNodeId])
                {
                    dist[edge.toNodeId] = alt;
                    prev[edge.toNodeId] = u;
                }
            }
        }

        var path = new List<NodeData>();
        string current = goalId;
        while (current != null)
        {
            var node = graph.GetNode(current);
            if (node == null) break;

            path.Insert(0, node);
            current = prev[current];
        }
        return path;
    }
    

    public static float CalculatePathCost(List<NodeData> path, TravelMode mode)
    {
        float total = 0f;
        for (int i = 0; i < path.Count - 1; i++)
        {
            var edge = path[i].edges.Find(e => e.toNodeId == path[i + 1].nodeId);
            if (edge != null)
            {
                total += (mode == TravelMode.Time) ? edge.energyCost : edge.moneyCost;
            }
        }
        return total;
    }
    

}

public enum TravelMode
{
    Time,
    Money
}
