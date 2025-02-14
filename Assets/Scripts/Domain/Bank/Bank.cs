using System.Collections.Generic;

public class Bank
{
    public string Name { get; private set; }
    public List<BankCard> Cards { get; private set; } = new List<BankCard>();

    public Bank(string name)
    {
        Name = name;
    }

    public void AddCard(BankCard card)
    {
        Cards.Add(card);
    }

    public List<BankCard> GetPlayerCards()
    {
        return Cards;
    }
}
