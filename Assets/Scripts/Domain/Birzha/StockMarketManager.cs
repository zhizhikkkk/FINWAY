using System.Collections.Generic;
using UnityEngine;

public class StockMarketManager : MonoBehaviour
{
    public List<Stock> availableStocks = new List<Stock>();
    private IStockTransactionStrategy transactionStrategy;

    private void Awake()
    {
        transactionStrategy = new SimpleTransactionStrategy();
    }

    private void Start()
    {
        availableStocks.Add(new Stock("KZKZ", "Kaspi.kz", 150f, new RandomPriceStrategy()));
        availableStocks.Add(new Stock("TGPK", "TechGroup", 80f, new RandomPriceStrategy()));
        availableStocks.Add(new Stock("OILX", "OilEx", 110f, new RandomPriceStrategy()));
    }

    public void UpdateStockPrices()
    {
        foreach (var stock in availableStocks)
        {
            float delta = stock.PriceStrategy.GetPriceChange(stock);
            float newPrice = stock.CurrentPrice + delta;
            stock.CurrentPrice = Mathf.Max(1f, newPrice);
        }
    }

    public bool BuyStock(PlayerModel player, string symbol, int quantity)
    {
        var stock = availableStocks.Find(s => s.Symbol == symbol);
        if (stock == null) return false;
        return transactionStrategy.Buy(player, stock, quantity);
    }

    public bool SellStock(PlayerModel player, string symbol, int quantity)
    {
        var stock = availableStocks.Find(s => s.Symbol == symbol);
        if (stock == null) return false;
        return transactionStrategy.Sell(player, stock, quantity);
    }
}
