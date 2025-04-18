using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class FinancialManagerUIController : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Button openFinancialManagerButton;
    [SerializeField] private GameObject financialManagerPanel;
    [SerializeField] private TMP_Text expenseLogText;
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
        var expenseLog = _expenseManager.GetExpenseLog();

        // Формируем строку для отображения
        string logText = "Expense Log:\n";
        foreach (var entry in expenseLog)
        {
            // Используем данные из PlayerModel для отображения даты в игровом формате
            string gameDate = $"Day {_playerModel.Days.Value}, Hour {_playerModel.Hours.Value}";
            logText += $"{gameDate} - {entry.Category}: {entry.Amount}$\n";
        }

        expenseLogText.text = logText;

        // Показываем общие расходы
        totalIncomeText.text = $"Total Income: {_playerModel.Cash.Value}$";
        totalExpenseText.text = $"Total Expenses: {_expenseManager.GetTotalExpenses()}$";
    }
}
