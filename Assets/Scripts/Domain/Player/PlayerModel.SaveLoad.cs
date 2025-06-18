using System.Collections.Generic;

public partial class PlayerModel
{
    public PlayerData ToData()
    {
        var d = new PlayerData
        {
            Cash = Cash.Value,
            Budget = Budget.Value,
            Energy = Energy.Value,
            Happiness = Happiness.Value,
            Days = Days.Value,
            Hours = Hours.Value,

            BankCards = new List<BankCard>(BankCards),
            WorkProgressList = new List<WorkProgressEntry>(),
            PortfolioList = new List<OwnedStockEntry>(),
            ExpenseLog = new List<ExpenseEntry>(ExpenseLog),
            IncomeLog = new List<IncomeEntry>(IncomeLog)
        };

        foreach (var kv in WorkProgressMap)
            d.WorkProgressList.Add(new WorkProgressEntry { JobId = kv.Key, Progress = kv.Value });

        foreach (var st in Portfolio.GetAllStocks())
            d.PortfolioList.Add(new OwnedStockEntry
            {
                Symbol = st.Symbol,
                CompanyName = st.CompanyName,
                OwnedShares = st.OwnedShares
            });

        return d;
    }

    public void ApplyData(PlayerData d)
    {
        Cash.Value = d.Cash;
        Budget.Value = d.Budget;
        Energy.Value = d.Energy;
        Happiness.Value = d.Happiness;
        Days.Value = d.Days;
        Hours.Value = d.Hours;

        BankCards.Clear(); BankCards.AddRange(d.BankCards);
        ExpenseLog.Clear(); ExpenseLog.AddRange(d.ExpenseLog);
        IncomeLog.Clear(); IncomeLog.AddRange(d.IncomeLog);

        WorkProgressMap.Clear();
        foreach (var wp in d.WorkProgressList)
            WorkProgressMap[wp.JobId] = wp.Progress;

        Portfolio.Clear();
        foreach (var p in d.PortfolioList)
            Portfolio.AddStock(p.Symbol, p.CompanyName, 0f, p.OwnedShares);

        RecalculateBudget();
    }
}
