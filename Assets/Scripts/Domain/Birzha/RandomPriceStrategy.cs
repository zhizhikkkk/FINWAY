public class RandomPriceStrategy : IPriceStrategy
{
    private float maxPriceChange = 10f; 
    public float GetPriceChange(Stock stock)
    {
        return UnityEngine.Random.Range(-maxPriceChange, maxPriceChange);
    }
}
