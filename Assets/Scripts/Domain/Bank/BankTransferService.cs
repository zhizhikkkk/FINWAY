using UnityEngine;

public class BankTransferService
{
    private PlayerModel _player;

    
    public BankTransferService(PlayerModel player)
    {
        _player = player;
    }

    public bool TransferMoney(string fromCardNumber, string toCardNumber, float amount)
    {
        BankCard fromCard = _player.BankCards.Find(card => card.CardNumber == fromCardNumber);
        BankCard toCard = _player.BankCards.Find(card => card.CardNumber == toCardNumber);

        if (fromCard == null)
        {
            Debug.LogError("Source card not found.");
            return false;
        }
        if (toCard == null)
        {
            Debug.LogError("Destination card not found.");
            return false;
        }

        float commissionRate = (fromCard.BankName != toCard.BankName) ? 0.01f : 0f;
        float commission = amount * commissionRate;
        float totalDeduction = amount + commission;

        if (fromCard.Balance < totalDeduction)
        {
            Debug.Log("Not enough balance on the source card.");
            return false;
        }

        bool success = fromCard.Withdraw(totalDeduction);
        if (!success)
        {
            Debug.LogError("Withdrawal from source card failed.");
            return false;
        }

        toCard.Deposit(amount);
        _player.RecalculateBudget();
        Debug.Log($"Transferred {amount} from {fromCard.CardNumber} to {toCard.CardNumber}. Commission: {commission}");

        return true;
    }
    public bool WithdrawToCash(string fromCardNumber, float amount)
    {
        BankCard fromCard = _player.BankCards.Find(card => card.CardNumber == fromCardNumber);

        if (fromCard == null)
        {
            Debug.LogError("Source card not found.");
            return false;
        }

        if (fromCard.Balance < amount)
        {
            Debug.Log("Not enough balance on the source card.");
            return false;
        }

        // Снимаем с карты
        bool success = fromCard.Withdraw(amount);
        if (!success)
        {
            Debug.LogError("Withdrawal from source card failed.");
            return false;
        }

        _player.Cash.Value += amount;

        _player.RecalculateBudget();

        Debug.Log($"Withdrew {amount} from card {fromCardNumber} to cash. Card balance: {fromCard.Balance}");
        return true;
    }

    public bool DepositFromCash(string toCardNumber, float amount)
    {
        BankCard toCard = _player.BankCards.Find(card => card.CardNumber == toCardNumber);

        if (toCard == null)
        {
            Debug.LogError("Destination card not found.");
            return false;
        }

        if (_player.Cash.Value < amount)
        {
            Debug.Log("Not enough cash on hand.");
            return false;
        }

        _player.Cash.Value -= amount;

        toCard.Deposit(amount);

        _player.RecalculateBudget();

        Debug.Log($"Deposited {amount} from cash to card {toCardNumber}. Card balance: {toCard.Balance}");
        return true;
    }

}
