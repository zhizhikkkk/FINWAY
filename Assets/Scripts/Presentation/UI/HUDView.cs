using UnityEngine;
using TMPro;
using UniRx;
using Zenject;

public class HUDView : MonoBehaviour
{
    public TMP_Text cashText;
    public TMP_Text budgetText;
    public TMP_Text energyText;
    public TMP_Text timeText;
    public TMP_Text happinessText;

    private PlayerModel _playerModel;
    private CompositeDisposable _disposables = new CompositeDisposable();

    [Inject]
    public void Construct(PlayerModel playerModel)
    {
        _playerModel = playerModel;
        _playerModel.Cash.Subscribe(UpdateCash).AddTo(_disposables);
        _playerModel.Budget.Subscribe(UpdateBudget).AddTo(_disposables);
        _playerModel.Energy.Subscribe(UpdateEnergy).AddTo(_disposables);
        _playerModel.Days.Subscribe(_ => UpdateTime()).AddTo(_disposables);
        _playerModel.Hours.Subscribe(_ => UpdateTime()).AddTo(_disposables);
        _playerModel.Happiness.Subscribe(UpdateHappiness).AddTo(_disposables);
    }

    private void Start()
    {
        UpdateCash(_playerModel.Cash.Value);
        UpdateBudget(_playerModel.Budget.Value);
        UpdateEnergy(_playerModel.Energy.Value);
        UpdateHappiness(_playerModel.Happiness.Value);
        UpdateTime();
    }

    private void OnDestroy()
    {
        _disposables.Dispose();
    }

    private void UpdateCash(float value)
    {
        cashText.text = $"{value}";
    }

    private void UpdateBudget(float value)
    {
        budgetText.text = $"{value}";
    }

    private void UpdateEnergy(float value)
    {
        energyText.text = $"{value}";
    }
    private void UpdateHappiness(float value)
    {
        happinessText.text = $"{Mathf.RoundToInt(value)}";
    }
    private void UpdateTime()
    {
        timeText.text = $"{_playerModel.Days.Value}d,{_playerModel.Hours.Value}h";
    }
}
