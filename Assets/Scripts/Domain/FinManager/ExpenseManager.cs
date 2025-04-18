using System.Collections.Generic;
using System;
public class ExpenseManager
{
    private PlayerModel _playerModel;

    private float dailyFoodExpense = 20f;
    private float weeklyRentExpense = 200f;
    private int lastExpenseDate;

    public ExpenseManager(PlayerModel playerModel)
    {
        _playerModel = playerModel;
        lastExpenseDate = _playerModel.Days.Value;
    }

    public void UpdateExpenses()
    {
        int currentDate = _playerModel.Days.Value;

        if (currentDate > lastExpenseDate)
        {
            for (int day = lastExpenseDate + 1; day <= currentDate; day++)
            {
                DeductDailyExpenses(day);

                if ((day % 7) == 0)
                {
                    DeductWeeklyExpenses(day);
                }
            }

            lastExpenseDate = currentDate;
        }
    }

    private void DeductDailyExpenses(int day)
    {
        _playerModel.Cash.Value -= dailyFoodExpense;
        _playerModel.AddExpense(new ExpenseEntry
        {
            Date = day,
            Category = "Food",
            Amount = dailyFoodExpense,
            Description = "Daily food expense"
        });
    }

    private void DeductWeeklyExpenses(int day)
    {
        _playerModel.Cash.Value -= weeklyRentExpense;
        _playerModel.AddExpense(new ExpenseEntry
        {
            Date = day,
            Category = "Rent",
            Amount = weeklyRentExpense,
            Description = "Weekly rent and utilities"
        });

        float totalWeeklyIncome = GetTotalIncomeForWeek(); 
        float taxAmount = totalWeeklyIncome * 0.20f; 

        _playerModel.Cash.Value -= taxAmount;
        _playerModel.AddExpense(new ExpenseEntry
        {
            Date = day,
            Category = "Tax",
            Amount = taxAmount,
            Description = "Weekly VAT (20% on income)"
        });
    }

    public void AddTransportExpense(float amount)
    {
        _playerModel.Cash.Value -= amount;
        _playerModel.AddExpense(new ExpenseEntry
        {
            Date = _playerModel.Days.Value,
            Category = "Transport",
            Amount = amount,
            Description = "Transport expense (between locations)"
        });
    }

    
    private float GetTotalIncomeForWeek()
    {
        float totalIncome = 0f;
        int currentDay = _playerModel.Days.Value;

        foreach (var income in _playerModel.IncomeLog)
        {
            if (currentDay - income.Date <= 7)
            {
                totalIncome += income.Amount;
            }
        }

        return totalIncome;
    }


    public List<ExpenseEntry> GetExpenseLog()
    {
        return _playerModel.ExpenseLog;
    }

    public float GetTotalExpenses()
    {
        float total = 0;
        foreach (var expense in _playerModel.ExpenseLog)
        {
            total += expense.Amount;
        }
        return total;
    }

    public float GetTotalIncome()
    {
        float total = 0;
        foreach (var income in _playerModel.IncomeLog)
        {
            total += income.Amount;
        }
        return total;
    }
}


[System.Serializable]
public class ExpenseEntry
{
    public int Date;
    public string Category;
    public float Amount;
    public string Description;
}
