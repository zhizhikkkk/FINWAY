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

        // ������� ��������� ���������� ������ �� JSON
        PlayerData savedData = _dataManager.Load();
        if (savedData != null)
        {
            // ���� ���������� ������ �������, ������ ������ � ��������� ��� ����
            PlayerModel = new PlayerModel();
            PlayerModel.Cash.Value = savedData.Cash;
            PlayerModel.Budget.Value = savedData.Budget;
            PlayerModel.Energy.Value = savedData.Energy;
            PlayerModel.Days.Value = savedData.Days;
            PlayerModel.Hours.Value = savedData.Hours;
            PlayerModel.BankCards = savedData.BankCards ?? new List<BankCard>();

            // ���� ������ ���������� ���� � ���������� ������ �� ������, ���������� ���
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
            // ���� ���������� ������ ���, ������ ����� ������ � ���������� ����������
            PlayerModel = new PlayerModel();
            Debug.Log("[GameManager] No saved data found. Using default values.");
        }
    }

    public void Initialize()
    {
        // ����� ����� ��������� �������������� �������������, ���� ���������.
    }
    public void SaveData()
    {
        _dataManager.Save(PlayerModel);
    }
}
