using UniRx;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public partial class PlayerModel
{
    public ReactiveProperty<float> Cash { get; private set; }
    public ReactiveProperty<float> Budget { get; private set; }

    public ReactiveProperty<float> Energy { get; private set; }

    public ReactiveProperty<float> Happiness { get; private set; }
    public ReactiveProperty<int> Days { get; private set; }
    public StockPortfolio Portfolio { get; private set; }

    public List<BankCard> BankCards { get;  set; }
    public List<ExpenseEntry> ExpenseLog { get; set; }
    public List<IncomeEntry> IncomeLog { get; set; }
    public Dictionary<string, float> WorkProgressMap { get; set; }
    public PlayerModel()
    {
        Cash = new ReactiveProperty<float>(1000f);
        Budget = new ReactiveProperty<float>(1000f); 
        Energy = new ReactiveProperty<float>(100f);
        Happiness = new ReactiveProperty<float>(50f);
        Days = new ReactiveProperty<int>(1);
        BankCards = new List<BankCard>();
        Portfolio = new StockPortfolio();
        WorkProgressMap = new Dictionary<string, float>();
        ExpenseLog = new List<ExpenseEntry>();
        IncomeLog = new List<IncomeEntry>();
    }

    public float GetWorkProgress(string jobId)
    {
        return WorkProgressMap.TryGetValue(jobId, out var value) ? value : 0f;
    }

    public void SetWorkProgress(string jobId, float value)
    {
        WorkProgressMap[jobId] = value;
    }
    public void RecalculateBudget()
    {
        float total = Cash.Value;
        foreach (var card in BankCards)
        {
            total += card.Balance;
        }
        Budget.Value = total;
    }
    public void ChangeEnergy(float amount)
    {
        Energy.Value += amount;
        if(Energy.Value <0f) {
            Energy.Value = 0f;
        }

        if(Energy.Value > 100f) {
            Energy.Value = 100f;
        }
    }
    public void ChangeHappiness(float amount)
    {
        Happiness.Value += amount;
        if (Happiness.Value < 0f)
        {
            Happiness.Value = 0f;
        }

        if (Happiness.Value > 100f)
        {
            Happiness.Value = 100f;
        }
    }
    public void AddDay()
    {
        Days.Value++;
    }

    public void AddBankCard(BankCard card)
    {
        BankCards.Add(card);
    }

    public void RemoveBankCard(BankCard card)
    {
        BankCards.Remove(card);
    }
    public void AddExpense(ExpenseEntry expense)
    {
        ExpenseLog.Add(expense);
    }
    public void AddIncome(IncomeEntry income)
    {
        IncomeLog.Add(income);
    }
    public void ResetToDefaults()
    {
        Cash.Value = 1000f;
        Budget.Value = 1000f;
        Energy.Value = 100f;
        Happiness.Value = 100f;
        Days.Value = 1;

        BankCards.Clear();
        Portfolio.Clear();                
        WorkProgressMap.Clear();
        ExpenseLog.Clear();
        IncomeLog.Clear();
    }

}
[System.Serializable]
public class IncomeEntry
{
    public int Date;     
    public float Amount;       
    public string Description;  
    public string Category;
}


