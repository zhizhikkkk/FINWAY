using System.Collections.Generic;
using UnityEngine; // чтобы использовать Random

[System.Serializable]
public class Stock
{
    public string Symbol;
    public string CompanyName;
    public float CurrentPrice;
    public IPriceStrategy PriceStrategy;

    public List<float> PriceHistory = new List<float>();

    public Stock(string symbol, string companyName, float currentPrice, IPriceStrategy priceStrategy)
    {
        Symbol = symbol;
        CompanyName = companyName;
        CurrentPrice = currentPrice;
        PriceStrategy = priceStrategy;

        float lastPrice = currentPrice;
        for (int i = 0; i < 30; i++)
        {
            float delta = Random.Range(-5f, 5f);
            lastPrice = Mathf.Max(1f, lastPrice + delta);
            PriceHistory.Add(lastPrice);
        }
    }

    public void AddDailyPrice(float newPrice)
    {
        CurrentPrice = newPrice;
        PriceHistory.Add(newPrice);
        if (PriceHistory.Count > 30)
        {
            PriceHistory.RemoveAt(0);
        }
    }
}
