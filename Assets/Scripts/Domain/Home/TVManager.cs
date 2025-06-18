using UnityEngine;
using TMPro;
using System.Collections;
using Zenject;

public class TVManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] TextMeshProUGUI tvText;

    [Header("Баланс")]
    [SerializeField] float happinessPerHour = 2f;
    [SerializeField] float energyPerHour = 1f;
    [SerializeField] float stopDistance = 0.3f;  
    [SerializeField] float realSecPerGameHour = 8f;     
     

    PlayerModel _player;
    Transform _viewer;        
    Coroutine _loop;
    bool _watching;
    float _timer;

    public bool IsWatching => _watching;

    [Inject] void Construct(PlayerModel model) => _player = model;

    public void StartWatching(Transform viewer)
    {

        if (_watching) return;
        Debug.Log("Начал смотреть");
        _viewer = viewer;
        _watching = true;
        _timer = 0f;
        _loop = StartCoroutine(WatchLoop());

        UpdateUi();
    }

    public void StopWatching()
    {
        if (!_watching) return;
        Debug.Log("Zakonchil смотреть");
        _watching = false;
        if (_loop != null) StopCoroutine(_loop);
        _loop = null;

        UpdateUi();
    }

    IEnumerator WatchLoop()
    {
        while (_watching)
        {
            Debug.Log(Vector3.Distance(_viewer.position, transform.position));
            if (Vector3.Distance(_viewer.position, transform.position) > stopDistance)
            {
                StopWatching();
                UpdateUi();
                yield break;
            }

            _timer += Time.unscaledDeltaTime;
            if (_timer >= realSecPerGameHour)
            {
                _timer -= realSecPerGameHour;

                _player.AddHours(1);
                _player.ChangeHappiness(happinessPerHour);
                _player.ChangeEnergy(-energyPerHour);
            }
            UpdateUi();
            yield return null;  
        }
    }

    void UpdateUi() =>
        tvText.text = _watching ? "Watching TV" : "";

    void OnDestroy() => StopWatching();
}
