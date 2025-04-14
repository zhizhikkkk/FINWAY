using UnityEngine;
using System.Collections.Generic;
using System;

public class StockMarketManager : MonoBehaviour
{
    public List<Stock> availableStocks = new List<Stock>();
    public StockPortfolio portfolio = new StockPortfolio();
    public PlayerModel playerModel;

    private void Start()
    {
        availableStocks.Add(new Stock("KZKZ", "Kaspi.kz", 150f));
        availableStocks.Add(new Stock("TGPK", "TechGroup", 80f));
        availableStocks.Add(new Stock("OILX", "OilEx", 110f));
    }

    public void BuyStock(string symbol, int amount)
    {
        Stock stock = availableStocks.Find(s => s.Symbol == symbol);
        if (stock == null) return;

        float totalPrice = stock.CurrentPrice * amount;
        if (playerModel.Cash.Value >= totalPrice)
        {
            playerModel.Cash.Value -= totalPrice;  // Уменьшаем деньги игрока

            Stock owned = portfolio.GetStock(symbol); // Проверка, есть ли такие акции уже в портфеле
            if (owned != null)
            {
                owned.OwnedShares += amount;  // Увеличиваем количество акций
            }
            else
            {
                Stock copy = new Stock(stock.Symbol, stock.CompanyName, stock.CurrentPrice);
                copy.OwnedShares = amount;  // Создаём новый объект акции
                portfolio.Stocks.Add(copy);  // Добавляем в портфель
            }

            Debug.Log($"Куплено {amount} акций {symbol} по цене {stock.CurrentPrice}");
        }
    }

    public void SellStock(string symbol, int amount)
    {
        Stock owned = portfolio.GetStock(symbol); // Находим акцию в портфеле
        if (owned == null || owned.OwnedShares < amount) return;  // Проверяем, есть ли достаточно акций

        Stock marketStock = availableStocks.Find(s => s.Symbol == symbol); // Получаем текущую цену акции
        float totalValue = marketStock.CurrentPrice * amount;

        playerModel.Cash.Value += totalValue;  // Прибавляем деньги от продажи

        owned.OwnedShares -= amount;  // Уменьшаем количество акций в портфеле

        if (owned.OwnedShares <= 0)
        {
            portfolio.Stocks.Remove(owned);  // Удаляем акцию, если их больше не осталось
        }

        Debug.Log($"Продано {amount} акций {symbol} по цене {marketStock.CurrentPrice}");
    }

    public void UpdateStockPrices()
    {
        foreach (var stock in availableStocks)
        {
            float change = UnityEngine.Random.Range(-10f, 10f);
            stock.CurrentPrice = Mathf.Max(1f, stock.CurrentPrice + change);
        }
    }
}
