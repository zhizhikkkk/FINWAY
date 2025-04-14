using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Zenject;
using System.Linq;

public class StockMarketUIController : MonoBehaviour
{
    [SerializeField] private Transform stockListContainer;
    [SerializeField] private GameObject stockRowPrefab;
    [SerializeField] private TMP_InputField quantityInput;
    [SerializeField] private TMP_Text portfolioText;
    [SerializeField] private Button updatePricesButton;

    private StockMarketManager marketManager;

    [Inject]
    public void Construct(StockMarketManager manager)
    {
        marketManager = manager;
    }

    private void Start()
    {
        RenderStockList();
        updatePricesButton.onClick.AddListener(() =>
        {
            marketManager.UpdateStockPrices();
            RenderStockList();
        });
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
            row.Setup(stock, quantityInput, marketManager);
        }

        RenderPortfolio();
    }

    private void RenderPortfolio()
    {
        string summary = " Portfolio:\n";
        foreach (var s in marketManager.portfolio.Stocks)
        {
            summary += $"{s.Symbol}: {s.OwnedShares} shares\n";
        }

        portfolioText.text = summary;  // Обновляем текст с портфелем
    }

}
