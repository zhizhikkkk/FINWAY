using System.Collections.Generic;

public class StockPortfolio
{
    private List<OwnedStock> ownedStocks = new List<OwnedStock>();

    public void AddStock(string symbol, string companyName, float buyPrice, int quantity)
    {
        var existing = GetStock(symbol);
        if (existing != null)
        {
            existing.OwnedShares += quantity;
        }
        else
        {
            ownedStocks.Add(new OwnedStock(symbol, companyName, quantity));
        }
    }

    public OwnedStock GetStock(string symbol)
    {
        return ownedStocks.Find(s => s.Symbol == symbol);
    }

    public void RemoveStock(string symbol)
    {
        ownedStocks.RemoveAll(s => s.Symbol == symbol);
    }

    public List<OwnedStock> GetAllStocks()
    {
        return ownedStocks;
    }
    public void Clear()
    {
        ownedStocks.Clear();
    }
}

public class OwnedStock
{
    public string Symbol;
    public string CompanyName;
    public int OwnedShares;

    public OwnedStock(string symbol, string name, int shares)
    {
        Symbol = symbol;
        CompanyName = name;
        OwnedShares = shares;
    }
}
