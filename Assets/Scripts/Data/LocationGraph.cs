using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Хранит список локаций (узлов, NodeData) и дорог (рёбер, EdgeData), которые их соединяют. Это основа вашего графа
/// </summary>
[CreateAssetMenu(menuName = "Finway/LocationGraph")]
public class LocationGraph : ScriptableObject
{
    public List<NodeData> nodes;

    public NodeData GetNode(string nodeId)
    {
        return nodes.Find(n => n.nodeId == nodeId);
    }
}

[System.Serializable]
public class NodeData
{
    public string nodeId;
    public Vector2 position; // Для отрисовки и/или анимации движения
    public List<EdgeData> edges;
}

[System.Serializable]
public class EdgeData
{
    public string toNodeId;
    public float timeCost;
    public float moneyCost;
    // Можно добавить и другие параметры (energyCost, distance и т.д.)
}
