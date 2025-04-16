using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class LineChart : MonoBehaviour
{
    [SerializeField] private RectTransform chartContainer;
    [SerializeField] private GameObject pointPrefab;
    [SerializeField] private Color lineColor = Color.white;

    private List<GameObject> spawnedPoints = new List<GameObject>();

    public void ShowData(List<float> priceHistory)
    {
        foreach (var obj in spawnedPoints)
        {
            Destroy(obj);
        }
        spawnedPoints.Clear();
   

        if (priceHistory == null || priceHistory.Count == 0) return;

        float maxPrice = Mathf.Max(priceHistory.ToArray());
        float minPrice = Mathf.Min(priceHistory.ToArray());
        float range = Mathf.Max(1f, maxPrice - minPrice); 

        float width = chartContainer.rect.width;
        float height = chartContainer.rect.height;

        for (int i = 0; i < priceHistory.Count; i++)
        {
            float normalizedY = (priceHistory[i] - minPrice) / range;
            float xPos = (i / (priceHistory.Count - 1f)) * width;
            float yPos = normalizedY * height;
            
            GameObject pointObj = Instantiate(pointPrefab, chartContainer);
            RectTransform rt = pointObj.GetComponent<RectTransform>();
            rt.anchoredPosition = new Vector2(xPos, yPos);
            spawnedPoints.Add(pointObj);

            if (i > 0)
            {
                Vector2 startPos = spawnedPoints[i - 1].GetComponent<RectTransform>().anchoredPosition;
                Vector2 endPos = rt.anchoredPosition;
                DrawLine(startPos, endPos);
            }
        }
    }

    private void DrawLine(Vector2 startPos, Vector2 endPos)
    {
        GameObject lineObj = new GameObject("Line", typeof(Image));
        lineObj.transform.SetParent(chartContainer, false);
        lineObj.GetComponent<Image>().color = lineColor;
        RectTransform rt = lineObj.GetComponent<RectTransform>();

        Vector2 dir = (endPos - startPos).normalized;
        float distance = Vector2.Distance(startPos, endPos);
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.zero;
        rt.sizeDelta = new Vector2(distance, 2f);
        rt.anchoredPosition = startPos + dir * distance * 0.5f;
        rt.localEulerAngles = new Vector3(0, 0, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg);

        spawnedPoints.Add(lineObj);
    }
}
