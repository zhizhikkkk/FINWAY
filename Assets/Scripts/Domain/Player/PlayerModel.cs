using UniRx;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerModel
{
    // Финансовые данные: наличка, бюджет, которые могут использоваться по-разному
    public ReactiveProperty<float> Cash { get; private set; }
    public ReactiveProperty<float> Budget { get; private set; }

    // Физическое состояние
    public ReactiveProperty<float> Energy { get; private set; }

    // Время игры: дни и часы
    public ReactiveProperty<int> Days { get; private set; }
    public ReactiveProperty<int> Hours { get; private set; }

    // Банковские карты игрока
    public List<BankCard> BankCards { get;  set; }

    public PlayerModel()
    {
        Cash = new ReactiveProperty<float>(1000f);
        Budget = new ReactiveProperty<float>(1000f); // Можно использовать либо Cash, либо Budget, зависит от логики
        Energy = new ReactiveProperty<float>(100f);
        Days = new ReactiveProperty<int>(1);
        Hours = new ReactiveProperty<int>(0);
        BankCards = new List<BankCard>();
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

    public void AddBankCard(BankCard card)
    {
        BankCards.Add(card);
    }

    public void RemoveBankCard(BankCard card)
    {
        BankCards.Remove(card);
    }
}
