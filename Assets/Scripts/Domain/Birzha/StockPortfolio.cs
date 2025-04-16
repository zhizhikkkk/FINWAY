using System.Collections.Generic;

public class StockPortfolio
{
    private List<OwnedStock> ownedStocks = new List<OwnedStock>();

    // Добавить новые акции (или увеличить уже существующие)
    public void AddStock(string symbol, string companyName, float buyPrice, int quantity)
    {
        var existing = GetStock(symbol);
        if (existing != null)
        {
            existing.OwnedShares += quantity;
            // при желании пересчитать среднюю цену покупки и т.д.
        }
        else
        {
            ownedStocks.Add(new OwnedStock(symbol, companyName, quantity));
        }
    }

    // Получить объект, описывающий владение акцией (если есть)
    public OwnedStock GetStock(string symbol)
    {
        return ownedStocks.Find(s => s.Symbol == symbol);
    }

    // Удалить акции из портфеля, если OwnedShares = 0
    public void RemoveStock(string symbol)
    {
        ownedStocks.RemoveAll(s => s.Symbol == symbol);
    }

    // Можно добавить метод для получения списка всего, чем владеет игрок
    public List<OwnedStock> GetAllStocks()
    {
        return ownedStocks;
    }
}

public class OwnedStock
{
    public string Symbol;
    public string CompanyName;
    public int OwnedShares;
    // можно добавить поле averageBuyPrice, если нужно

    public OwnedStock(string symbol, string name, int shares)
    {
        Symbol = symbol;
        CompanyName = name;
        OwnedShares = shares;
    }
}
