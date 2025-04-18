using System.Linq;
using TMPro;
using UnityEngine;
using System.Collections.Generic;
using System;
using Unity.VisualScripting;
using Zenject;
using UnityEngine.UI;

public class FinancialManagerUIController : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Button openFinancialManagerButton;
    [SerializeField] private GameObject financialManagerPanel;
    [SerializeField] private TMP_Text expenseLogText;
    [SerializeField] private TMP_Text incomeLogText;
    [SerializeField] private TMP_Text totalIncomeText;
    [SerializeField] private TMP_Text totalExpenseText;

    private ExpenseManager _expenseManager;
    private PlayerModel _playerModel;

    [Inject]
    public void Construct(ExpenseManager expenseManager, PlayerModel playerModel)
    {
        _expenseManager = expenseManager;
        _playerModel = playerModel;
    }

    private void Start()
    {
        openFinancialManagerButton.onClick.AddListener(OpenFinancialManager);

        // Скрываем панель по умолчанию
        financialManagerPanel.SetActive(false);
    }

    private void Update()
    {
        // Обновляем расходы каждый день
        _expenseManager.UpdateExpenses();
    }

    private void OpenFinancialManager()
    {
        // Показываем панель финансового отчета
        financialManagerPanel.SetActive(true);
        RenderExpenseLog();
    }

    public void CloseFinancialManager()
    {
        financialManagerPanel.SetActive(false);
    }

    private void RenderExpenseLog()
    {
        // Получаем все расходы
        var expenseLog = SummarizeLog(_expenseManager.GetExpenseLog());

        // Формируем строку для отображения
        string logText = "Expense Log:\n";
        foreach (var entry in expenseLog)
        {
            // Используем данные из PlayerModel для отображения даты в игровом формате
            string gameDate = $"Day {entry.Date}";
            logText += $"{gameDate} - {entry.Category}: {entry.Amount}$\n";
        }

        expenseLogText.text = logText;

        // Показываем общие расходы
        totalIncomeText.text = $"Total Income: {_playerModel.Cash.Value}$";
        totalExpenseText.text = $"Total Expenses: {_expenseManager.GetTotalExpenses()}$";

        // Получаем все доходы
        var incomeLog = SummarizeLog(_playerModel.IncomeLog);

        // Формируем строку для доходов
        string incomeText = "Income Log:\n";
        foreach (var entry in incomeLog)
        {
            string gameDate = $"Day {entry.Date}";
            incomeText += $"{gameDate} - {entry.Category}: {entry.Amount}$ ({entry.Category})\n";
        }

        // Отображаем информацию о доходах
        incomeLogText.text = incomeText;
    }

    // Метод для суммирования всех расходов по категориям и дням
    private List<LogEntry> SummarizeLog(List<ExpenseEntry> entries)
    {
        var summary = new List<LogEntry>();

        foreach (var entry in entries)
        {
            var existingEntry = summary.FirstOrDefault(e => e.Date == entry.Date && e.Category == entry.Category);
            if (existingEntry != null)
            {
                existingEntry.Amount += entry.Amount;
            }
            else
            {
                summary.Add(new LogEntry
                {
                    Date = entry.Date,
                    Category = entry.Category,
                    Amount = entry.Amount
                });
            }
        }

        return summary;
    }

    // Метод для суммирования всех доходов по категориям и дням
    private List<LogEntry> SummarizeLog(List<IncomeEntry> entries)
    {
        var summary = new List<LogEntry>();

        foreach (var entry in entries)
        {
            var existingEntry = summary.FirstOrDefault(e => e.Date == entry.Date && e.Category == entry.Category);
            if (existingEntry != null)
            {
                existingEntry.Amount += entry.Amount;
            }
            else
            {
                summary.Add(new LogEntry
                {
                    Date = entry.Date,
                    Category = entry.Category,
                    Amount = entry.Amount
                });
            }
        }

        return summary;
    }
}

// Новый класс для объединения логов (расходов и доходов)
public class LogEntry
{
    public int Date { get; set; }
    public string Category { get; set; }
    public float Amount { get; set; }
}
