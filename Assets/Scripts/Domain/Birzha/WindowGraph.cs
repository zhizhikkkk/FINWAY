using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WindowGraph : MonoBehaviour
{
    [SerializeField] private Sprite circleSprite;
    [SerializeField] private Color lineColor = Color.white;
    private RectTransform graphContainer;

    private readonly List<GameObject> spawned = new();

    private void Awake()
    {
        graphContainer = transform.Find("graphContainer").GetComponent<RectTransform>();
    }

    private GameObject CreateCircle(Vector2 anchoredPos, int idx)
    {
        GameObject go = new($"Circle {idx + 1}", typeof(Image));
        go.transform.SetParent(graphContainer, false);
        go.GetComponent<Image>().sprite = circleSprite;

        RectTransform rt = go.GetComponent<RectTransform>();
        rt.anchoredPosition = anchoredPos;
        rt.sizeDelta = new Vector2(11, 11);
        rt.anchorMin = rt.anchorMax = Vector2.zero;
        spawned.Add(go);                         
        return go;
    }

    public void ShowGraph(List<float> valueList)
    {
        if (valueList == null || valueList.Count == 0) return;

        ClearOldGraph();                        

        float h = graphContainer.sizeDelta.y;
        float w = graphContainer.sizeDelta.x;
        float xStep = w / Mathf.Max(1, valueList.Count - 1);

        float yMax = Mathf.Max(valueList.ToArray());
        float yMin = Mathf.Min(valueList.ToArray());
        float yRange = Mathf.Max(1f, yMax - yMin);

        GameObject prev = null;
        for (int i = 0; i < valueList.Count; i++)
        {
            float x = i * xStep;
            float y = ((valueList[i] - yMin) / yRange) * h;
            GameObject curr = CreateCircle(new Vector2(x, y), i);

            if (prev != null)
                CreateLine(prev.GetComponent<RectTransform>().anchoredPosition,
                           curr.GetComponent<RectTransform>().anchoredPosition);

            prev = curr;
        }
    }

    private void CreateLine(Vector2 a, Vector2 b)
    {
        GameObject go = new("Line", typeof(Image));
        go.transform.SetParent(graphContainer, false);
        go.GetComponent<Image>().color = lineColor;

        RectTransform rt = go.GetComponent<RectTransform>();
        Vector2 dir = (b - a).normalized;
        float dist = Vector2.Distance(a, b);

        rt.sizeDelta = new Vector2(dist, 2f);         
        rt.anchorMin = rt.anchorMax = Vector2.zero;
        rt.anchoredPosition = a + dir * dist * 0.5f;          
        rt.localEulerAngles = new Vector3(0, 0,
                                 Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg);
        spawned.Add(go);                                      
    }

    private void ClearOldGraph()
    {
        foreach (var go in spawned) Destroy(go);
        spawned.Clear();
    }
}
