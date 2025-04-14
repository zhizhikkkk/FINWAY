using System.Collections.Generic;

public class StockPortfolio
{
    public List<Stock> Stocks = new List<Stock>();

    public Stock GetStock(string symbol)
    {
        return Stocks.Find(s => s.Symbol == symbol);
    }
}
