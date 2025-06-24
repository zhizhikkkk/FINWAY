public interface IStockTransactionStrategy
{
    bool Buy(PlayerModel player, Stock stock, int quantity);

    bool Sell(PlayerModel player, Stock stock, int quantity);
}
