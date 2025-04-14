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
            playerModel.Cash.Value -= totalPrice;  // ��������� ������ ������

            Stock owned = portfolio.GetStock(symbol); // ��������, ���� �� ����� ����� ��� � ��������
            if (owned != null)
            {
                owned.OwnedShares += amount;  // ����������� ���������� �����
            }
            else
            {
                Stock copy = new Stock(stock.Symbol, stock.CompanyName, stock.CurrentPrice);
                copy.OwnedShares = amount;  // ������ ����� ������ �����
                portfolio.Stocks.Add(copy);  // ��������� � ��������
            }

            Debug.Log($"������� {amount} ����� {symbol} �� ���� {stock.CurrentPrice}");
        }
    }

    public void SellStock(string symbol, int amount)
    {
        Stock owned = portfolio.GetStock(symbol); // ������� ����� � ��������
        if (owned == null || owned.OwnedShares < amount) return;  // ���������, ���� �� ���������� �����

        Stock marketStock = availableStocks.Find(s => s.Symbol == symbol); // �������� ������� ���� �����
        float totalValue = marketStock.CurrentPrice * amount;

        playerModel.Cash.Value += totalValue;  // ���������� ������ �� �������

        owned.OwnedShares -= amount;  // ��������� ���������� ����� � ��������

        if (owned.OwnedShares <= 0)
        {
            portfolio.Stocks.Remove(owned);  // ������� �����, ���� �� ������ �� ��������
        }

        Debug.Log($"������� {amount} ����� {symbol} �� ���� {marketStock.CurrentPrice}");
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
