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

    [Header("Graph Data")]
    [SerializeField] private LocationGraph locationGraph;

    private PlayerLocationManager _locationManager;
    private PlayerModel _playerModel;
    private PlayerDataManager _playerDataManager;
    private ExpenseManager _expenseManager;

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
        ExpenseManager expenseManager) 
    {
        _locationManager = locationManager;
        _playerModel = playerModel;
        _playerDataManager = dataManager;
        _expenseManager = expenseManager;

        Debug.Log($"[MapUIController] Injected PlayerLocationManager: {_locationManager}");
    }

    private void OnEnable()
    {
        if (_playerModel == null || _locationManager == null)
        {
            Debug.LogWarning("[MapUIController] Dependencies not injected yet. Forcing injection.");
            ProjectContext.Instance.Container.Inject(this);
        }

    }

    private void Start()
    {
        choosePanel.SetActive(false);
        Debug.Log($"[MapUIController] Start - currentLocation={_locationManager.CurrentLocation}, budget={_playerModel.Budget.Value}");
    }

    public void OnLocationButtonClicked(string locationId)
    {
        int hour = _playerModel.Hours.Value;

        _targetLocation = locationId;
        string from = _locationManager.CurrentLocation;
        if (from == _targetLocation)
        {
            _locationManager.SetLocation(_targetLocation);
            SceneManager.LoadScene(_targetLocation);
            return;
        }

        _busPath = LocationPathfinding.Dijkstra(locationGraph, from, _targetLocation, TravelMode.Money);
        _busCost = (_busPath != null && _busPath.Count > 1)
            ? LocationPathfinding.CalculatePathCost(_busPath, TravelMode.Money)
            : float.MaxValue;

        _walkPath = LocationPathfinding.Dijkstra(locationGraph, from, _targetLocation, TravelMode.Time);
        _walkCost = (_walkPath != null && _walkPath.Count > 1)
            ? LocationPathfinding.CalculatePathCost(_walkPath, TravelMode.Time)
            : float.MaxValue;

        ShowChoosePanel();
    }

    private void ShowChoosePanel()
    {
        string busStr = _busCost == float.MaxValue ? "N/A" : $"{_busCost:0} $";
        string walkStr = _walkCost == float.MaxValue ? "N/A" : $"{_walkCost:0} e";
        infoText.text = $"Travel from {_locationManager.CurrentLocation} to {_targetLocation}?\n" +
                        $"Bus cost:  {busStr}\n" +
                        $"Walk cost: {walkStr}\n" +
                        $"Choose an option:";
        choosePanel.SetActive(true);
    }

    public void OnMoneyConfirm()
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

    public void OnEnergyConfirm()
    {
        if (_walkCost == float.MaxValue)
        {
            Debug.Log("Нет пешеходного маршрута!");
            return;
        }

        _playerModel.ChangeEnergy(-Mathf.CeilToInt(_walkCost));
        _playerDataManager.Save(_playerModel);

        _locationManager.SetLocation(_targetLocation);
        SceneManager.LoadScene(_targetLocation);
    }

    public void OnExitClicked()
    {
        choosePanel.SetActive(false);
    }
}
