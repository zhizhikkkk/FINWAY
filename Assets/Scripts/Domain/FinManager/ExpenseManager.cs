using System.Collections.Generic;
using System;

public class ExpenseManager
{
    private PlayerModel _playerModel;

    private float dailyFoodExpense = 20f;
    private float weeklyRentExpense = 200f;
    private float dailyTransportExpense = 10f;

    private DateTime lastExpenseDate;

    public ExpenseManager(PlayerModel playerModel)
    {
        _playerModel = playerModel;
        lastExpenseDate = DateTime.Now;
    }

    public void UpdateExpenses()
    {
        DateTime currentDate = DateTime.Now;

        // Если прошёл новый день
        if (currentDate.Date > lastExpenseDate.Date)
        {
            DeductDailyExpenses();

            if ((currentDate - lastExpenseDate).Days >= 7)
            {
                DeductWeeklyExpenses();
            }

            lastExpenseDate = currentDate; // обновляем дату последнего расчёта
        }
    }

    private void DeductDailyExpenses()
    {
        _playerModel.Cash.Value -= dailyFoodExpense;
        _playerModel.AddExpense(new ExpenseEntry
        {
            Date = DateTime.Now,
            Category = "Food",
            Amount = dailyFoodExpense,
            Description = "Daily food expense"
        });

        // Снять расходы на транспорт
        _playerModel.Cash.Value -= dailyTransportExpense;
        _playerModel.AddExpense(new ExpenseEntry
        {
            Date = DateTime.Now,
            Category = "Transport",
            Amount = dailyTransportExpense,
            Description = "Daily transport expense"
        });
    }


    private void DeductWeeklyExpenses()
    {
        _playerModel.Cash.Value -= weeklyRentExpense;
        _playerModel.ExpenseLog.Add(new ExpenseEntry
        {
            Date = DateTime.Now,
            Category = "Rent",
            Amount = weeklyRentExpense,
            Description = "Weekly rent and utilities"
        });
    }

    // Метод для добавления расходов на транспорт
    public void AddTransportExpense(float amount)
    {
        _playerModel.Cash.Value -= amount;
        _playerModel.ExpenseLog.Add(new ExpenseEntry
        {
            Date = DateTime.Now,
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
}


[System.Serializable]
public class ExpenseEntry
{
    public DateTime Date;
    public string Category;
    public float Amount;
    public string Description;
}
