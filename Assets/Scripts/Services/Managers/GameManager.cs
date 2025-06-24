using Zenject;
using UniRx;
using UnityEngine;
using System.Collections.Generic;

public class GameManager : IInitializable
{
    public static GameManager Instance { get; private set; }
    public PlayerModel PlayerModel { get; private set; }

    private PlayerDataManager _dataManager;

    [Inject]
    public void Construct(PlayerDataManager dataManager,PlayerModel playerModel)
    {
        Debug.Log("GameManager");
        Instance = this;
        _dataManager = dataManager;

        PlayerData savedData = _dataManager.Load();
        if (savedData != null)
        {
            PlayerModel = playerModel;
            PlayerModel.Cash.Value = savedData.Cash;
            PlayerModel.Budget.Value = savedData.Budget;
            PlayerModel.Energy.Value = savedData.Energy;
            PlayerModel.Happiness.Value = savedData.Happiness;

            PlayerModel.Days.Value = savedData.Days;
            PlayerModel.Hours.Value = savedData.Hours;
            PlayerModel.BankCards = savedData.BankCards ?? new List<BankCard>();
            PlayerModel.WorkProgressMap = new Dictionary<string, float>();
            if (savedData.WorkProgressList != null)
            {
                foreach (var entry in savedData.WorkProgressList)
                {
                    PlayerModel.WorkProgressMap[entry.JobId] = entry.Progress;
                }
            }
            if (savedData.PortfolioList != null)
            {
                foreach (var ownedEntry in savedData.PortfolioList)
                {
                    PlayerModel.Portfolio.AddStock(
                        ownedEntry.Symbol,
                        ownedEntry.CompanyName,
                        0f,
                        ownedEntry.OwnedShares
                    );
                }
            }
            if (savedData.ExpenseLog != null)
            {
                foreach (var expense in savedData.ExpenseLog)
                {
                    PlayerModel.AddExpense(expense);
                }
            }
            if (savedData.IncomeLog != null)
            {
                foreach (var income in savedData.IncomeLog)
                {
                    PlayerModel.AddIncome(income);
                }
            }
            Debug.Log($"[GameManager] Loaded saved data. Cash: {PlayerModel.Cash.Value}, Cards: {PlayerModel.BankCards.Count}");
        }
        else
        {
            PlayerModel = new PlayerModel();
            Debug.Log("[GameManager] No saved data found. Using default values.");
        }
    }

    public void Initialize()
    {
        
    }

    public void SaveData()
    {
        _dataManager.Save(PlayerModel);
    }
}
