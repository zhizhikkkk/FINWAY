using System.Collections.Generic;

[System.Serializable]
public class PlayerData
{
    public float Cash;
    public float Budget;
    public float Energy;
    public float Happiness;

    public int Days;
    public List<BankCard> BankCards;
    public List<WorkProgressEntry> WorkProgressList;
    public List<OwnedStockEntry> PortfolioList;
    public List<ExpenseEntry> ExpenseLog;
    public List<IncomeEntry> IncomeLog;

}
[System.Serializable]
public class WorkProgressEntry
{
    public string JobId;
    public float Progress;
}
[System.Serializable]
public class OwnedStockEntry
{
    public string Symbol;
    public string CompanyName;
    public int OwnedShares;
}
