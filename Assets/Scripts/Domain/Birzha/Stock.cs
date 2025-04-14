[System.Serializable]
public class Stock
{
    public string Symbol; 
    public string CompanyName; 
    public float CurrentPrice;
    public float OwnedShares;

    public Stock(string symbol, string name, float price)
    {
        Symbol = symbol;
        CompanyName = name;
        CurrentPrice = price;
        OwnedShares = 0;
    }
}
