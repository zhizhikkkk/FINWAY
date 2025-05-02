using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Zenject;

public class StockDetailPanel : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text currentPriceText;
    [SerializeField] private WindowGraph windowGraph;
    [SerializeField] private TMP_InputField quantityInput;
    [SerializeField] private Button buyButton;
    [SerializeField] private Button sellButton;
    [SerializeField] private Button closeButton;

    private Stock currentStock;
    private StockMarketManager manager;
    private PlayerModel player;

    [Inject]
    public void Construct(StockMarketManager managerRef, PlayerModel playerRef)
    {
        manager = managerRef;
        player = playerRef; 
    }

    private void Awake()
    {
        gameObject.SetActive(false);

        buyButton.onClick.AddListener(OnBuyClicked);
        sellButton.onClick.AddListener(OnSellClicked);

        closeButton.onClick.AddListener(OnCloseClicked);
    }

    public void Show(Stock stock)
    {
        currentStock = stock;

        titleText.text = $"{stock.CompanyName} ({stock.Symbol})";
        currentPriceText.text = $"${stock.CurrentPrice:F2}";

        gameObject.SetActive(true);
        if (windowGraph && stock.PriceHistory != null && stock.PriceHistory.Count > 0)
        {
            windowGraph.ShowGraph(stock.PriceHistory);
        }
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void OnCloseClicked()
    {
        Hide();
    }

    private void OnBuyClicked()
    {
        if (currentStock == null || manager == null || player == null) return;

        int qty = 1;
        if (quantityInput && !string.IsNullOrEmpty(quantityInput.text))
        {
            int.TryParse(quantityInput.text, out qty);
        }
        manager.BuyStock(player, currentStock.Symbol, qty);

        Debug.Log($"[StockDetailPanel] Attempted to BUY {qty} shares of {currentStock.Symbol}");
    }

    private void OnSellClicked()
    {
        if (currentStock == null || manager == null || player == null) return;

        int qty = 1;
        if (quantityInput && !string.IsNullOrEmpty(quantityInput.text))
        {
            int.TryParse(quantityInput.text, out qty);
        }
        manager.SellStock(player, currentStock.Symbol, qty);

        Debug.Log($"[StockDetailPanel] Attempted to SELL {qty} shares of {currentStock.Symbol}");
    }
}
