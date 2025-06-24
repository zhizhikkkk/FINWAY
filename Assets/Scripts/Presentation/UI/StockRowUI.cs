using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StockRowUI : MonoBehaviour
{
    [SerializeField] private TMP_Text symbolText;
    [SerializeField] private TMP_Text priceText;
    [SerializeField] private Button infoButton;

    private Stock stock;
    private StockDetailPanel detailPanel;

    public void Setup(Stock stockData, StockDetailPanel detailPanelRef)
    {
        stock = stockData;
        detailPanel = detailPanelRef;

        symbolText.text = $"{stock.CompanyName} ({stock.Symbol})";
        priceText.text = $"${stock.CurrentPrice:F2}";

        infoButton.onClick.RemoveAllListeners();
        infoButton.onClick.AddListener(ShowDetail);
    }

    private void ShowDetail()
    {
        if (detailPanel != null && stock != null)
        {
            detailPanel.Show(stock);
        }
    }
}
