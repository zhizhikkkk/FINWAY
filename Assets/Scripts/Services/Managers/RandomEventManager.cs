using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Zenject;

public class RandomEventManager : IInitializable, IDisposable
{
    private readonly PlayerModel _player;
    private readonly CompositeDisposable _disposables = new CompositeDisposable();
    private readonly List<RandomEvent> _events = new List<RandomEvent>();
    private readonly System.Random _rng = new System.Random();

    public RandomEventManager(PlayerModel player)
    {
        _player = player;
       
        _events.Add(new RandomEvent(
            name: "CoffeeMachineBroken",
            description: "� ���������� � ����� �������� ��������. ������ -- 50$.",
            chance: 1f,
            apply: () =>
            {
                float cost = 50f;
                _player.Cash.Value -= cost;
                _player.AddExpense(new ExpenseEntry
                {
                    Date = _player.Days.Value,
                    Category = "Random Event(������ ����������)",
                    Amount = cost,
                    Description = "������ ����������"
                });
                Debug.Log("[Event] Coffee machine broken: -50$");
            }
        ));

        _events.Add(new RandomEvent(
            name: "FoundMoneyOnStreet",
            description: "�� ����� �� ����� ������ � ��������!",
            chance: 1f,
            apply: () =>
            {
                float gain = UnityEngine.Random.Range(10f, 100f);
                _player.Cash.Value += gain;
                _player.AddIncome(new IncomeEntry
                {
                    Date = _player.Days.Value,
                    Category = "Random Event(����� ������ �� �����)",
                    Amount = gain,
                    Description = "����� ������ �� �����"
                });
                Debug.Log($"[Event] Found money on street: +{gain}$");
            }
        ));

        _events.Add(new RandomEvent(
            name: "TrafficFine",
            description: "����� �� ������������ ��������: 30$.",
            chance: 1f,
            apply: () =>
            {
                float fine = 30f;
                _player.Cash.Value -= fine;
                _player.AddExpense(new ExpenseEntry
                {
                    Date = _player.Days.Value,
                    Category = "Random Event(����� �����)",
                    Amount = fine,
                    Description = "����� �����"
                });
                Debug.Log("[Event] Traffic fine: -30$");
            }
        ));

        _events.Add(new RandomEvent(
            name: "BirthdayGift",
            description: "�� �������� ������� �� ���� ��������: 100$.",
            chance: 1f,
            apply: () =>
            {
                float gift = 100f;
                _player.Cash.Value += gift;
                _player.AddIncome(new IncomeEntry
                {
                    Date = _player.Days.Value,
                    Category = "Random Event(������� �� ���� ��������)",
                    Amount = gift,
                    Description = "������� �� ���� ��������"
                });
                Debug.Log("[Event] Birthday gift: +100$");
            }
        ));
    }

    public void Initialize()
    {
        Debug.Log("[RandomEventManager] Initialize() called");
        _player.Days
            .Skip(1)
            .Subscribe(_ =>
            {
                Debug.Log($"[RandomEventManager] Day changed to {_player.Days.Value}, trying event�");
                TryTriggerEvent();
            })
            .AddTo(_disposables);
    }


    private void TryTriggerEvent()
    {
        foreach (var ev in _events)
        {
            if (_rng.NextDouble() < ev.Chance)
            {
                ev.Apply();
                break;
            }
        }
    }

    public void Dispose()
    {
        _disposables.Dispose();
    }

    private class RandomEvent
    {
        public string Name { get; }
        public string Description { get; }
        public float Chance { get; }
        private readonly Action _apply;
        public RandomEvent(string name, string description, float chance, Action apply)
        {
            Name = name;
            Description = description;
            Chance = chance;
            _apply = apply;
        }
        public void Apply() => _apply();
    }
}
