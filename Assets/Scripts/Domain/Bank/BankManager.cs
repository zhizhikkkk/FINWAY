using System.Collections.Generic;
using UnityEngine;

public class BankManager
{
    public List<Bank> Banks { get; private set; } = new List<Bank>();
    
    
    public BankManager(PlayerModel player, PlayerDataManager dataManager, GameManager gameManager)
    {
        Debug.Log("SALAM");
        // ������� ��������� ���������� ������
        PlayerData savedData = dataManager.Load();
        if (savedData != null && savedData.BankCards != null && savedData.BankCards.Count > 0)
        {
            // ��������������� ����� �� ���������� ������
            foreach (var card in savedData.BankCards)
            {
                // ����� ��� ������� ���� ��� ���� �����
                Bank bank = Banks.Find(b => b.Name == card.BankName);
                if (bank == null)
                {
                    bank = new Bank(card.BankName);
                    Banks.Add(bank);
                }
                bank.AddCard(card);
                player.BankCards.Add(card);
                dataManager.Save(player);
                gameManager.PlayerModel = player;
            }
        }
        else
        {
            // ���� ���������� ������ ���, ������ ����� �� ���������
            player.BankCards.Add(CreateBank("Kaspi"));
            player.BankCards.Add(CreateBank("Halyk"));
            player.BankCards.Add(CreateBank("Jusan"));
            dataManager.Save(player);
            gameManager.PlayerModel = player;

        }
    }

    public BankCard CreateBank(string name)
    {
        Bank newBank = new Bank(name);
        string cardNumber = CardGenerator.GenerateCardNumber();
        string cvv = CardGenerator.GenerateCVV();
        string expDate = CardGenerator.GenerateExpirationDate();

        var newCard = new BankCard(name, cardNumber, expDate, cvv);
        newCard.Balance = 10000f;
        newBank.AddCard(newCard);
        Banks.Add(newBank);
        return newCard;
    }

    public Bank GetBank(string bankName)
    {
        return Banks.Find(b => b.Name == bankName);
    }

    public List<string> GetBankNames()
    {
        var names = new List<string>();
        foreach (var bank in Banks)
        {
            names.Add(bank.Name);
        }
        return names;
    }
}
