using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class StockRowUI : MonoBehaviour
{
    [SerializeField] private TMP_Text symbolText;
    [SerializeField] private TMP_Text priceText;
    [SerializeField] private Button buyButton;
    [SerializeField] private Button sellButton;

    private Stock stock;

    public void Setup(Stock stockData, TMP_InputField quantityInput, StockMarketManager marketManager)
    {
        stock = stockData;

        symbolText.text = $"{stock.CompanyName} ({stock.Symbol})";
        priceText.text = $"${stock.CurrentPrice:F2}";

        buyButton.onClick.AddListener(() =>
        {
            // Проверяем, является ли введённое значение числом
            if (int.TryParse(quantityInput.text, out int qty) && qty > 0)
            {
                marketManager.BuyStock(stock.Symbol, qty);
            }
            else
            {
                Debug.LogWarning("Invalid quantity input");
            }
        });

        sellButton.onClick.AddListener(() =>
        {
            // Проверяем, является ли введённое значение числом
            if (int.TryParse(quantityInput.text, out int qty) && qty > 0)
            {
                marketManager.SellStock(stock.Symbol, qty);
            }
            else
            {
                Debug.LogWarning("Invalid quantity input");
            }
        });
    }

}
