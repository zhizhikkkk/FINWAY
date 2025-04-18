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
    }

    public void AddTransportExpense(float amount)
    {
        _playerModel.Cash.Value -= amount;
        _playerModel.ExpenseLog.Add(new ExpenseEntry
        {
            Date = _playerModel.Days.Value,
            Category = "Transport",
            Amount = amount,
            Description = "Transport expense (between locations)"
        });
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
