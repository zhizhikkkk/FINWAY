using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Zenject;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using Zenject.SpaceFighter;

public class MapUIController : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject choosePanel;
    [SerializeField] private TMP_Text infoText;
    [SerializeField] private Button moneyButton;
    [SerializeField] private Button timeButton;
    [SerializeField] private Button exitButton;

    [Header("Graph Data")]
    [SerializeField] private LocationGraph locationGraph;

    // Инъекции
    private PlayerLocationManager _locationManager;
    private PlayerModel _playerModel;
    private PlayerDataManager _playerDataManager;
    private ExpenseManager _expenseManager;
    private LocationAvailabilityService _avail;  // <-- сервис для проверок

    private string _targetLocation;
    private float _busCost;
    private float _walkCost;
    private List<NodeData> _busPath;
    private List<NodeData> _walkPath;

    [Inject]
    public void Construct(
        PlayerLocationManager locationManager,
        PlayerModel playerModel,
        PlayerDataManager dataManager,
        ExpenseManager expenseManager,
        LocationAvailabilityService availabilityService)  // <-- получили сервис
    {
        _locationManager = locationManager;
        _playerModel = playerModel;
        _playerDataManager = dataManager;
        _expenseManager = expenseManager;
        _avail = availabilityService;

        Debug.Log($"[MapUIController] Injected PlayerLocationManager: {_locationManager}");
    }

    private void OnEnable()
    {
        // в случае, если вдруг инъекция ещё не произошла
        if (_playerModel == null || _locationManager == null)
        {
            Debug.LogWarning("[MapUIController] Dependencies not injected yet. Forcing injection.");
            ProjectContext.Instance.Container.Inject(this);
        }

        moneyButton.onClick.AddListener(OnMoneyConfirm);
        timeButton.onClick.AddListener(OnTimeConfirm);
        exitButton.onClick.AddListener(OnExitClicked);
    }

    private void Start()
    {
        choosePanel.SetActive(false);
        Debug.Log($"[MapUIController] Start - currentLocation={_locationManager.CurrentLocation}, budget={_playerModel.Budget.Value}");
    }

    /// <summary>
    /// Вызывается из OnClick() кнопок локаций (заданы в Инспекторе) с нужным ID.
    /// </summary>
    public void OnLocationButtonClicked(string locationId)
    {
        int hour = _playerModel.Hours.Value;

        // 1) Проверяем, открыта ли локация в данный час
        if (!_avail.IsOpen(locationId, hour))
        {
            Debug.LogWarning($"Локация «{locationId}» сейчас закрыта! Работает с {_avail.GetOpenHour(locationId)} до {_avail.GetCloseHour(locationId)}.");
            return;
        }

        // 2) Если это та же локация — перезагрузить
        _targetLocation = locationId;
        string from = _locationManager.CurrentLocation;
        if (from == _targetLocation)
        {
            _locationManager.SetLocation(_targetLocation);
            SceneManager.LoadScene(_targetLocation);
            return;
        }

        // 3) Рассчитываем автобусный маршрут (деньги)
        _busPath = LocationPathfinding.Dijkstra(locationGraph, from, _targetLocation, TravelMode.Money);
        _busCost = (_busPath != null && _busPath.Count > 1)
            ? LocationPathfinding.CalculatePathCost(_busPath, TravelMode.Money)
            : float.MaxValue;

        // 4) Рассчитываем пешеходный маршрут (время)
        _walkPath = LocationPathfinding.Dijkstra(locationGraph, from, _targetLocation, TravelMode.Time);
        _walkCost = (_walkPath != null && _walkPath.Count > 1)
            ? LocationPathfinding.CalculatePathCost(_walkPath, TravelMode.Time)
            : float.MaxValue;

        // 5) Открываем панель выбора способа
        ShowChoosePanel();
    }

    private void ShowChoosePanel()
    {
        string busStr = _busCost == float.MaxValue ? "N/A" : $"{_busCost:0} $";
        string walkStr = _walkCost == float.MaxValue ? "N/A" : $"{_walkCost:0} h";
        infoText.text = $"Travel from {_locationManager.CurrentLocation} to {_targetLocation}?\n" +
                        $"Bus cost:  {busStr}\n" +
                        $"Walk cost: {walkStr}\n" +
                        $"Choose an option:";
        choosePanel.SetActive(true);
    }

    private void OnMoneyConfirm()
    {
        if (_busCost == float.MaxValue)
        {
            Debug.Log("Нет автобусного маршрута!");
            return;
        }
        if (_playerModel.Cash.Value < _busCost)
        {
            Debug.Log("Недостаточно денег на автобус!");
            return;
        }

        _playerModel.Cash.Value -= _busCost;
        _playerModel.RecalculateBudget();
        _expenseManager.AddTransportExpense(_busCost);
        _playerDataManager.Save(_playerModel);

        _locationManager.SetLocation(_targetLocation);
        SceneManager.LoadScene(_targetLocation);
    }

    private void OnTimeConfirm()
    {
        if (_walkCost == float.MaxValue)
        {
            Debug.Log("Нет пешеходного маршрута!");
            return;
        }

        _playerModel.AddHours(Mathf.CeilToInt(_walkCost));
        _playerDataManager.Save(_playerModel);

        _locationManager.SetLocation(_targetLocation);
        SceneManager.LoadScene(_targetLocation);
    }

    private void OnExitClicked()
    {
        choosePanel.SetActive(false);
    }
}
