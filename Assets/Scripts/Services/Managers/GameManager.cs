using Zenject;
using UniRx;
using UnityEngine;
using System.Collections.Generic;

public class GameManager : IInitializable
{
    public static GameManager Instance { get; private set; }

    public PlayerModel PlayerModel { get;  set; }
    private PlayerDataManager _dataManager;

    [Inject]
    public void Construct(PlayerDataManager dataManager)
    {
        Instance = this;
        _dataManager = dataManager;

        // Попытка загрузить сохранённые данные из JSON
        PlayerData savedData = _dataManager.Load();
        if (savedData != null)
        {
            // Если сохранённые данные найдены, создаём модель и заполняем все поля
            PlayerModel = new PlayerModel();
            PlayerModel.Cash.Value = savedData.Cash;
            PlayerModel.Budget.Value = savedData.Budget;
            PlayerModel.Energy.Value = savedData.Energy;
            PlayerModel.Days.Value = savedData.Days;
            PlayerModel.Hours.Value = savedData.Hours;
            PlayerModel.BankCards = savedData.BankCards ?? new List<BankCard>();

            // Если список банковских карт в сохранённых данных не пустой, используем его
            if (savedData.BankCards != null)
            {
                PlayerModel.BankCards = savedData.BankCards;
            }
            else
            {
                PlayerModel.BankCards = new List<BankCard>();
            }

            Debug.Log($"[GameManager] Loaded saved data. Cash: {PlayerModel.Cash.Value}");
        }
        else
        {
            // Если сохранённых данных нет, создаём новую модель с дефолтными значениями
            PlayerModel = new PlayerModel();
            Debug.Log("[GameManager] No saved data found. Using default values.");
        }
    }

    public void Initialize()
    {
        // Здесь можно выполнить дополнительную инициализацию, если требуется.
    }
    public void SaveData()
    {
        _dataManager.Save(PlayerModel);
    }
}
