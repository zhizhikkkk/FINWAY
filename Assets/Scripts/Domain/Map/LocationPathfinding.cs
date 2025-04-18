using System.Collections.Generic;
using UnityEngine;


public static class LocationPathfinding
{
    /// <summary>
    /// Находит путь (список узлов) от startId к goalId,
    /// используя либо timeCost, либо moneyCost (зависит от travelMode).
    /// </summary>
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
                                 ? edge.timeCost
                                 : edge.moneyCost;

                float alt = dist[u] + edgeCost;

                if (alt < dist[edge.toNodeId])
                {
                    dist[edge.toNodeId] = alt;
                    prev[edge.toNodeId] = u;
                }
            }
        }

        // Восстановление пути
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
    

    // Подсчитываем сумму cost (Time или Money) вдоль пути
    public static float CalculatePathCost(List<NodeData> path, TravelMode mode)
    {
        float total = 0f;
        for (int i = 0; i < path.Count - 1; i++)
        {
            var edge = path[i].edges.Find(e => e.toNodeId == path[i + 1].nodeId);
            if (edge != null)
            {
                total += (mode == TravelMode.Time) ? edge.timeCost : edge.moneyCost;
            }
        }
        return total;
    }
    

}

public enum TravelMode
{
    Time,   // Игнорируем moneyCost, учитываем только timeCost
    Money   // Игнорируем timeCost, учитываем только moneyCost
}
