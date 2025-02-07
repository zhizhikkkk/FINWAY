using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Zenject;
using UniRx;

public class HUDView : MonoBehaviour
{
    public TextMeshProUGUI BudgetText;
    public TextMeshProUGUI EnergyText;
    public TextMeshProUGUI TimeText; 

    private PlayerModel _playerModel;
    private CompositeDisposable _disposables = new CompositeDisposable();

    [Inject]
    public void Construct(PlayerModel playerModel)
    {
        _playerModel = playerModel;

        _playerModel.Budget.Subscribe(UpdateBudget).AddTo(_disposables);
        _playerModel.Energy.Subscribe(UpdateEnergy).AddTo(_disposables);
        _playerModel.Days.Subscribe(_ => UpdateTime()).AddTo(_disposables);
        _playerModel.Hours.Subscribe(_ => UpdateTime()).AddTo(_disposables);
    }

    private void Start()
    {
        UpdateBudget(_playerModel.Budget.Value);
        UpdateEnergy(_playerModel.Energy.Value);
        UpdateTime();
    }

    private void OnDestroy()
    {
        _disposables.Dispose();
    }

    private void UpdateBudget(float budget)
    {
        BudgetText.text = $"Ѕюджет: {budget}";
    }

    private void UpdateEnergy(float energy)
    {
        EnergyText.text = $"Ёнерги€: {energy}";
    }

    private void UpdateTime()
    {
        TimeText.text = $"¬рем€: ƒень {_playerModel.Days.Value}, „асы {_playerModel.Hours.Value}";
    }
}
