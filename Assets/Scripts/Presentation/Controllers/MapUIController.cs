using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Zenject;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

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

    private PlayerLocationManager _locationManager;
    private PlayerModel _playerModel;

    private string _targetLocation;
    private float _busCost;
    private float _walkCost;
    private List<NodeData> _busPath;
    private List<NodeData> _walkPath;

    [Inject]
    public void Construct(PlayerLocationManager locationManager, PlayerModel playerModel)
    {
        _locationManager = locationManager;
        _playerModel = playerModel;
        Debug.Log($"[MapUIController] Injected PlayerLocationManager: {_locationManager}");
    }

    private void OnEnable()
    {
        if (_locationManager == null || _playerModel == null)
        {
            Debug.LogWarning("[MapUIController] Injecting dependencies late.");
            ProjectContext.Instance.Container.Inject(this); 
        }

        moneyButton.onClick.AddListener(OnMoneyConfirm);
        timeButton.onClick.AddListener(OnTimeConfirm);
        exitButton.onClick.AddListener(OnExitClicked);
    }

    private void Start()
    {
        Debug.Log($"[MapUIController] Start - _locationManager={_locationManager?.CurrentLocation}, _playerModel={_playerModel?.Budget.Value}");
        choosePanel.SetActive(false);
    }

    public void OnLocationButtonClicked(string locationId)
    {
        Debug.Log($"To: {locationId}");
        _targetLocation = locationId;
        string from = _locationManager.CurrentLocation;
        Debug.Log($"From: {from}");

        if (from == _targetLocation)
        {
            _locationManager.SetLocation(_targetLocation);
            SceneManager.LoadScene(_targetLocation);
            return;
        }

        _busPath = LocationPathfinding.Dijkstra(locationGraph, from, _targetLocation, TravelMode.Money);
        float busDist = (_busPath != null && _busPath.Count > 1) ? LocationPathfinding.CalculatePathCost(_busPath, TravelMode.Money) : float.MaxValue;

        _walkPath = LocationPathfinding.Dijkstra(locationGraph, from, _targetLocation, TravelMode.Time);
        float walkDist = (_walkPath != null && _walkPath.Count > 1) ? LocationPathfinding.CalculatePathCost(_walkPath, TravelMode.Time) : float.MaxValue;

        _busCost = busDist;
        _walkCost = walkDist;
        choosePanel.SetActive(true);

        string busStr = (_busCost == float.MaxValue) ? "N/A" : $"{_busCost} $";
        string walkStr = (_walkCost == float.MaxValue) ? "N/A" : $"{_walkCost} h";

        infoText.text = $"Travel from {from} to {_targetLocation}?\n" +
                        $"Bus cost: {busStr}\n" +
                        $"Walk cost: {walkStr}\n" +
                        $"Choose an option:";
    }

    private void OnMoneyConfirm()
    {
        if (_busCost == float.MaxValue)
        {
            Debug.Log("No bus route available!");
            return;
        }

        if (_playerModel.Budget.Value < _busCost)
        {
            Debug.Log("Not enough money!");
            return;
        }
        Debug.Log("Budget: " + _playerModel.Budget);
        _playerModel.ChangeBudget(-_busCost);
        Debug.Log("Budget: " + _playerModel.Budget);
        _locationManager.SetLocation(_targetLocation);
        SceneManager.LoadScene(_targetLocation);
    }

    private void OnTimeConfirm()
    {
        if (_walkCost == float.MaxValue)
        {
            Debug.Log("No walk route available!");
            return;
        }

        _playerModel.AddHours((int)_walkCost);

        _locationManager.SetLocation(_targetLocation);
        SceneManager.LoadScene(_targetLocation);
    }

    private void OnExitClicked()
    {
        choosePanel.SetActive(false);
    }
}
