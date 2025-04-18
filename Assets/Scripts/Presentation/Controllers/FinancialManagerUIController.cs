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

        // �������� ������ �� ���������
        financialManagerPanel.SetActive(false);
    }

    private void Update()
    {
        // ��������� ������� ������ ����
        _expenseManager.UpdateExpenses();
    }

    private void OpenFinancialManager()
    {
        // ���������� ������ ����������� ������
        financialManagerPanel.SetActive(true);
        RenderExpenseLog();
    }

    public void CloseFinancialManager()
    {
        financialManagerPanel.SetActive(false);
    }

    private void RenderExpenseLog()
    {
        // �������� ��� �������
        var expenseLog = _expenseManager.GetExpenseLog();

        // ��������� ������ ��� �����������
        string logText = "Expense Log:\n";
        foreach (var entry in expenseLog)
        {
            // ���������� ������ �� PlayerModel ��� ����������� ���� � ������� �������
            string gameDate = $"Day {_playerModel.Days.Value}, Hour {_playerModel.Hours.Value}";
            logText += $"{gameDate} - {entry.Category}: {entry.Amount}$\n";
        }

        expenseLogText.text = logText;

        // ���������� ����� �������
        totalIncomeText.text = $"Total Income: {_playerModel.Cash.Value}$";
        totalExpenseText.text = $"Total Expenses: {_expenseManager.GetTotalExpenses()}$";
    }
}
