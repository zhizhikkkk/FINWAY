using UniRx;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerModel
{
    // ���������� ������: �������, ������, ������� ����� �������������� ��-�������
    public ReactiveProperty<float> Cash { get; private set; }
    public ReactiveProperty<float> Budget { get; private set; }

    // ���������� ���������
    public ReactiveProperty<float> Energy { get; private set; }

    // ����� ����: ��� � ����
    public ReactiveProperty<int> Days { get; private set; }
    public ReactiveProperty<int> Hours { get; private set; }

    // ���������� ����� ������
    public List<BankCard> BankCards { get;  set; }

    public PlayerModel()
    {
        Cash = new ReactiveProperty<float>(1000f);
        Budget = new ReactiveProperty<float>(1000f); // ����� ������������ ���� Cash, ���� Budget, ������� �� ������
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
