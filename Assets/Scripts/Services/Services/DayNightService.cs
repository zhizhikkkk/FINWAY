using System;
using UniRx;
using UnityEngine;
using Zenject;

public enum DayPeriod { Day, Night }

public class DayNightService : IInitializable, IDisposable
{
    const int DayStart = 7;    // 07:00
    const int NightStart = 22; // 22:00

    private PlayerModel _player;
    public IReadOnlyReactiveProperty<bool> IsDay => _isDay;
    public IReadOnlyReactiveProperty<DayPeriod> CurrentPeriod => _period;
    private ReactiveProperty<bool> _isDay = new ReactiveProperty<bool>();
    private ReactiveProperty<DayPeriod> _period = new ReactiveProperty<DayPeriod>();
    private CompositeDisposable _disp = new CompositeDisposable();

    [Inject]
    public DayNightService(PlayerModel player)
    {
        _player = player;
    }

    public void Initialize()
    {
        Observable.Merge(
            _player.Days,     
            _player.Hours    
        )
        .Subscribe(_ => UpdatePeriod())
        .AddTo(_disp);

        UpdatePeriod(); 
    }

    void UpdatePeriod()
    {
        int h = _player.Hours.Value;
        bool isDay = (h >= DayStart && h < NightStart);
        _isDay.Value = isDay;
        _period.Value = isDay ? DayPeriod.Day : DayPeriod.Night;
    }

    public void Dispose()
    {
        _disp.Dispose();
    }
}
