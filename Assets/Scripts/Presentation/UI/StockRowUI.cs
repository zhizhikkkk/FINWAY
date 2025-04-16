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

        // Устанавливаем текст
        symbolText.text = $"{stock.CompanyName} ({stock.Symbol})";
        priceText.text = $"${stock.CurrentPrice:F2}";

        // На всякий случай очищаем старые слушатели (если были)
        infoButton.onClick.RemoveAllListeners();
        // Добавляем новый
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
