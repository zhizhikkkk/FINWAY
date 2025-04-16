using System.Collections.Generic;
using UnityEngine;

public class BankManager
{
    private PlayerModel _player;
    public List<Bank> Banks { get; private set; } = new List<Bank>();

    public BankManager(PlayerModel player)
    {
        _player = player;

        if (_player.BankCards.Count == 0)
        {
            _player.BankCards.Add(CreateBank("Kaspi"));
            _player.BankCards.Add(CreateBank("Halyk"));
            _player.BankCards.Add(CreateBank("Jusan"));
        }
        else
        {
            foreach (var card in _player.BankCards)
            {
                Bank existingBank = Banks.Find(b => b.Name == card.BankName);
                if (existingBank == null)
                {
                    existingBank = new Bank(card.BankName);
                    Banks.Add(existingBank);
                }
                existingBank.AddCard(card);
            }
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
