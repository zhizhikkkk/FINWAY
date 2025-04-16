using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Zenject;

public class StockMarketUIController : MonoBehaviour
{
    [SerializeField] private Transform stockListContainer;
    [SerializeField] private GameObject stockRowPrefab;

    private StockMarketManager marketManager;
    [SerializeField] private StockDetailPanel detailPanel;

    [Inject]
    public void Construct(StockMarketManager manager)
    {
        marketManager = manager;
    }

    private void Start()
    {
        RenderStockList();
    }

    private void RenderStockList()
    {
        foreach (Transform child in stockListContainer)
        {
            Destroy(child.gameObject);
        }

        foreach (var stock in marketManager.availableStocks)
        {
            var go = Instantiate(stockRowPrefab, stockListContainer);
            var row = go.GetComponent<StockRowUI>();
            row.Setup(stock, detailPanel);
            Debug.Log(row);
        }
    }

    
}
