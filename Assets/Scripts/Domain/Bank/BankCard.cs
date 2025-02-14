using UnityEngine;

[System.Serializable]
public class BankCard
{
    public string BankName;         // Название банка (например, "Kaspi")
    public string CardNumber;       // 16-значный номер карты
    public string ExpirationDate;   // Срок действия, формат "MM/YY"
    public string CVV;              // 3-значный CVV
    public float Balance;           // Баланс на карте
    public bool IsCreditCard;       // Если true, карта кредитная (для будущего расширения)
    public float CreditLimit { get; private set; }
    public float Debt { get; private set; }


    // Конструктор для дебетовой карты (по умолчанию)
    public BankCard(string bankName, string cardNumber, string expirationDate, string cvv)
    {
        BankName = bankName;
        CardNumber = cardNumber;
        ExpirationDate = expirationDate;
        CVV = cvv;
        IsCreditCard = false;
        Balance = 0f;
        Debt = 0;
    }

    public void Deposit(float amount)
    {
        Balance += amount;
    }

    public bool Withdraw(float amount)
    {
        if (Balance >= amount)
        {
            Balance -= amount;
            return true;
        }
        return false;
    }
    public bool TakeLoan(float amount)
    {
        if (IsCreditCard && (Debt + amount) <= CreditLimit)
        {
            Debt += amount;
            Balance += amount;
            return true;
        }
        return false;
    }

    public void RepayLoan(float amount)
    {
        if (Debt > 0)
        {
            float payAmount = Mathf.Min(amount, Debt);
            Balance -= payAmount;
            Debt -= payAmount;
        }
    }
}
