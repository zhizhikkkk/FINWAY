

using UnityEngine;

public class SimpleTransactionStrategy : IStockTransactionStrategy
{
    public bool Buy(PlayerModel player, Stock stock, int quantity)
    {
        float totalCost = stock.CurrentPrice * quantity;
        if (player.Cash.Value >= totalCost)
        {
            player.Cash.Value -= totalCost;
            player.Portfolio.AddStock(stock.Symbol, stock.CompanyName, stock.CurrentPrice, quantity);
            Debug.Log($"Bought {quantity} shares of {stock.Symbol} for {totalCost}");
            return true;
        }
        else
        {
            Debug.LogWarning("Not enough money to buy!");
            return false;
        }
    }

    public bool Sell(PlayerModel player, Stock stock, int quantity)
    {
        var owned = player.Portfolio.GetStock(stock.Symbol);
        if (owned == null || owned.OwnedShares < quantity)
        {
            Debug.LogWarning("Not enough shares to sell!");
            return false;
        }

        float totalIncome = stock.CurrentPrice * quantity;
        player.Cash.Value += totalIncome;

        owned.OwnedShares -= quantity;
        if (owned.OwnedShares <= 0)
        {
            player.Portfolio.RemoveStock(stock.Symbol);
        }

        Debug.Log($"Sold {quantity} shares of {stock.Symbol} for {totalIncome}");
        return true;
    }
}
