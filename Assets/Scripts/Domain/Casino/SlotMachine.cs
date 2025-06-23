using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SlotMachine : MonoBehaviour
{
    public enum Symbol { Seven, Cherry, Lemon, Bell }

    [Header("UI Refs")]
    [SerializeField] private GameObject uiPanel;       
    [SerializeField] private TMP_Text resultText;
    [SerializeField] private TMP_Text betText;
    [SerializeField] private Button spinButton;
    [SerializeField] private Button closeButton;

    private PlayerModel _player;
    private int _betAmount = 20;

    void Awake()
    {
        uiPanel.SetActive(false);
        spinButton.onClick.AddListener(OnSpinClicked);
        closeButton.onClick.AddListener(CloseUI);
    }

    public void StartGame(PlayerModel player)
    {
        _player = player;
        if (_player.Cash.Value < _betAmount)
        {
            ShowMessage("Not enough cash to play!");
            return;
        }

        betText.text = $"Bet: {_betAmount}$";
        resultText.text = "Good luck!";
        uiPanel.SetActive(true);
    }

    private void OnSpinClicked()
    {
        _player.Cash.Value -= _betAmount;
        betText.text = $"Bet: {_betAmount}$";

        Symbol[] reels = new Symbol[3];
        for (int i = 0; i < 3; i++)
            reels[i] = (Symbol)Random.Range(0, System.Enum.GetValues(typeof(Symbol)).Length);

        resultText.text = $"{reels[0]} | {reels[1]} | {reels[2]}";

        int multiplier = EvaluatePayout(reels);
        if (multiplier > 0)
        {
            int winnings = _betAmount * multiplier;
            _player.Cash.Value += winnings;
            ShowMessage($"You win x{multiplier}! +{winnings}$");
        }
        else
        {
            ShowMessage("No win, try again!");
        }
    }

    private int EvaluatePayout(Symbol[] r)
    {
        if (r[0] == Symbol.Seven && r[1] == Symbol.Seven && r[2] == Symbol.Seven)
            return 5;

        if (r[0] == Symbol.Bell && r[1] == Symbol.Bell && r[2] == Symbol.Bell)
            return 3;

        if (r[0] == Symbol.Cherry && r[1] == Symbol.Cherry && r[2] == Symbol.Cherry)
            return 2;

        int sevenCount = 0;
        foreach (var s in r) if (s == Symbol.Seven) sevenCount++;
        if (sevenCount == 2) return 1;

        return 0;
    }

    private void ShowMessage(string msg)
    {
        resultText.text = msg;
    }

    public void CloseUI()
    {
        uiPanel.SetActive(false);
    }

    public static void CloseAnyOpenUI()
    {
        foreach (var machine in FindObjectsOfType<SlotMachine>())
            machine.CloseUI();
    }
}
