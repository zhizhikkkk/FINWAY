using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ������ ������ ������� (�����, NodeData) � ����� (����, EdgeData), ������� �� ���������. ��� ������ ������ �����
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
    public Vector2 position; // ��� ��������� �/��� �������� ��������
    public List<EdgeData> edges;
}

[System.Serializable]
public class EdgeData
{
    public string toNodeId;
    public float timeCost;
    public float moneyCost;
    // ����� �������� � ������ ��������� (energyCost, distance � �.�.)
}
