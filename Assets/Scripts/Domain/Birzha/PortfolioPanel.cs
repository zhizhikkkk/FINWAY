using UnityEngine;
using TMPro;
using Zenject;

public class PortfolioPanel : MonoBehaviour
{
    [SerializeField] private TMP_Text portfolioText;
    [SerializeField] private GameObject panel; // сам контейнер панельки

    private PlayerModel playerModel;

    [Inject]
    public void Construct(PlayerModel playerRef)
    {
        playerModel = playerRef;
    }

    private void Awake()
    {
         panel.SetActive(false);
    }

    // Показываем панель и обновляем текст
    public void Show()
    {
       panel.SetActive(true);

        // Собираем список акций
        var ownedStocks = playerModel.Portfolio.GetAllStocks();
        if (ownedStocks.Count == 0)
        {
            portfolioText.text = "No stocks owned.";
            return;
        }

        // Формируем строку
        string result = "Your Portfolio:\n";
        foreach (var owned in ownedStocks)
        {
            result += $"{owned.CompanyName} ({owned.Symbol}) x {owned.OwnedShares}\n";
        }
        portfolioText.text = result;
    }

    // Скрываем панель
    public void Hide()
    {
        panel.SetActive(false);
    }
}
