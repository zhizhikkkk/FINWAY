using System.Collections;
using TMPro;
using UnityEngine;
using Zenject;

public class SleepManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] TextMeshProUGUI sleepText;
    [Header("Баланс сна")]
    [SerializeField] float energyPerHour = 2f;
    [SerializeField] float happinessPerHour = 10f;
    [SerializeField] float stopDistance = 13f;   
    [SerializeField] float realSecPerGameHour = 3f;     
    [SerializeField] float maxEnergy;

    PlayerModel _player;
    [Inject] void Construct(PlayerModel model) => _player = model;

    public bool IsSleeping => _sleeping;
    bool _sleeping;
    Transform _sleeper;         
    Coroutine _loop;
    float _timer;

    public void StartSleeping(Transform sleeper)
    {
        if (_sleeping || _player.Energy.Value >= maxEnergy) return;

        _sleeper = sleeper;
        _sleeping = true;
        _timer = 0f;
        _loop = StartCoroutine(SleepLoop());
        UpdateUi();
    }

    public void StopSleeping()
    {
        if (!_sleeping) return;

        _sleeping = false;
        if (_loop != null) StopCoroutine(_loop);
        _loop = null;
        UpdateUi();
    }

    IEnumerator SleepLoop()
    {
        while (_sleeping)
        {
            if (Vector3.Distance(_sleeper.position, transform.position) > stopDistance)
            {
                StopSleeping();
                UpdateUi();
                yield break;
            }

            _timer += Time.unscaledDeltaTime;
            if (_timer >= realSecPerGameHour)
            {
                _timer -= realSecPerGameHour;

                _player.AddHours(1);
                _player.ChangeEnergy(energyPerHour);
                _player.ChangeHappiness(happinessPerHour);

                if (_player.Energy.Value >= maxEnergy)
                {
                    _player.Energy.Value = maxEnergy;
                    StopSleeping();
                    yield break;
                }
            }
            UpdateUi();
            yield return null;
        }
    }
    void UpdateUi() =>
        sleepText.text = _sleeping ? "Sleeping" : "";
    void OnDestroy() => StopSleeping();
}
