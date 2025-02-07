using UniRx;
using UnityEngine;

public class PlayerModel
{
    public ReactiveProperty<float> Budget { get; private set; }
    public ReactiveProperty<float> Energy { get; private set; }
    public ReactiveProperty<int> Days { get; private set; }
    public ReactiveProperty<int> Hours { get; private set; }

    public PlayerModel()
    {
        Budget = new ReactiveProperty<float>(1000f);
        Energy = new ReactiveProperty<float>(100f);
        Days = new ReactiveProperty<int>(1); // Начинаем с 1 дня
        Hours = new ReactiveProperty<int>(0);
    }

    public void ChangeBudget(float amount)
    {
        Budget.Value += amount;
    }

    public void ChangeEnergy(float amount)
    {
        Energy.Value += amount;
    }

    public void AddHours(int hoursToAdd)
    {
        Hours.Value += hoursToAdd;

        while (Hours.Value >= 24)
        {
            Hours.Value -= 24;
            Days.Value += 1;
        }
    }
}
