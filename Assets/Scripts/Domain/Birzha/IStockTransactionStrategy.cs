public interface IStockTransactionStrategy
{
    // Покупка
    bool Buy(PlayerModel player, Stock stock, int quantity);

    // Продажа
    bool Sell(PlayerModel player, Stock stock, int quantity);
}
