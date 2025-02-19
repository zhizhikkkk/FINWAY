using UnityEngine;

public class BankTransferService
{
    private PlayerModel _player;

    // ������� ������ ������, ��� �������� ������ ����.
    public BankTransferService(PlayerModel player)
    {
        _player = player;
    }

    /// <summary>
    /// ������� ����� � ����� ����� �� ������.
    /// </summary>
    /// <param name="fromCardNumber">����� �����, � ������� ���������</param>
    /// <param name="toCardNumber">����� �����, �� ������� ���������</param>
    /// <param name="amount">����� ��������</param>
    /// <returns>true, ���� ������� ��������, false � ���� ��������� ������ (��������, ������������ �������)</returns>
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

        // ���������� ��������: ���� ����� ��������, �������� 1%, ����� 0%.
        float commissionRate = (fromCard.BankName != toCard.BankName) ? 0.01f : 0f;
        float commission = amount * commissionRate;
        float totalDeduction = amount + commission;

        if (fromCard.Balance < totalDeduction)
        {
            Debug.Log("Not enough balance on the source card.");
            return false;
        }

        // ������� ������ � �����-���������
        bool success = fromCard.Withdraw(totalDeduction);
        if (!success)
        {
            Debug.LogError("Withdrawal from source card failed.");
            return false;
        }

        // ��������� �����-����������
        toCard.Deposit(amount);
        _player.RecalculateBudget();
        Debug.Log($"Transferred {amount} from {fromCard.CardNumber} to {toCard.CardNumber}. Commission: {commission}");

        return true;
    }
}
