using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class PlayerDataManager
{
    private string filePath;

    public PlayerDataManager()
    {
        filePath = Path.Combine(Application.persistentDataPath, "playerData.json");
    }

    public void Save(PlayerModel playerModel)
    {
        PlayerData data = new PlayerData
        {
            Cash = playerModel.Cash.Value,
            Budget = playerModel.Budget.Value,
            Energy = playerModel.Energy.Value,
            Happiness = playerModel.Happiness.Value,
            Days = playerModel.Days.Value,
            Hours = playerModel.Hours.Value,
            BankCards = playerModel.BankCards,
            WorkProgressList = new List<WorkProgressEntry>(),
            PortfolioList = new List<OwnedStockEntry>(),
            ExpenseLog = new List<ExpenseEntry>(),
            IncomeLog = new List<IncomeEntry>()
        };

        foreach (var pair in playerModel.WorkProgressMap)
        {
            data.WorkProgressList.Add(new WorkProgressEntry
            {
                JobId = pair.Key,
                Progress = pair.Value
            });
        }

        var ownedStocks = playerModel.Portfolio.GetAllStocks();
        foreach (var owned in ownedStocks)
        {
            data.PortfolioList.Add(new OwnedStockEntry
            {
                Symbol = owned.Symbol,
                CompanyName = owned.CompanyName,
                OwnedShares = owned.OwnedShares
            });
        }

        foreach (var expense in playerModel.ExpenseLog)
        {
            data.ExpenseLog.Add(expense);
        }
        foreach (var income in playerModel.IncomeLog)
        {
            data.IncomeLog.Add(income);  
        }
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(filePath, json);
        Debug.Log("Player data saved:\n" + json);
    }

    public PlayerData Load()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            Debug.Log("Player data loaded:\n" + json);
            return JsonUtility.FromJson<PlayerData>(json);
        }
        else
        {
            Debug.Log("No save file found, using default data.");
            return null;
        }
    }
    public void SaveDefaults()
    {
        var data = new PlayerData
        {
            Cash = 1000f,
            Budget = 1000f,
            Energy = 100f,
            Happiness = 100f,
            Days = 1,
            Hours = 0,

            BankCards = new List<BankCard>(),
            WorkProgressList = new List<WorkProgressEntry>(),
            PortfolioList = new List<OwnedStockEntry>(),
            ExpenseLog = new List<ExpenseEntry>(),
            IncomeLog = new List<IncomeEntry>()
        };

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(filePath, json);
        Debug.Log($"[PlayerDataManager] Defaults saved:\n{json}");
    }
    public void ResetData()
    {
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
            Debug.Log("Player save data reset.");
        }
    }

}
