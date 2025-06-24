using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class BankSelectionUI : MonoBehaviour
{
    [Header("UI References")]
    public Transform bankButtonContainer;
    public Button bankButtonPrefab;

    public GameObject singleCardPanel;
    public TextMeshProUGUI cardInfoText;
    public Button transferButton;
    public Button closeSingleCardPanelButton;

    public Button withdrawButton;   
    public Button depositButton;    

    public GameObject transferPanel;
    public TextMeshProUGUI cardName;
    public TextMeshProUGUI cardNumber;
    public TextMeshProUGUI cardAmount;
    public TextMeshProUGUI statusText;
    public TMP_InputField toCardInput;
    public TMP_InputField amountInput;
    public Button confirmTransferButton;
    public Button cancelTransferButton;

    public GameObject withdrawPanel;
    public TMP_InputField withdrawAmountInput;
    public Button confirmWithdrawButton;
    public Button cancelWithdrawButton;

    public GameObject depositPanel;
    public TMP_InputField depositAmountInput;
    public Button confirmDepositButton;
    public Button cancelDepositButton;

    private BankManager _bankManager;
    private PlayerModel _player;
    private BankTransferService _transferService;
    private Bank _selectedBank;
    private BankCard _selectedCard;
    private GameManager _gameManager;
    private PlayerDataManager _dataManager;

    [Inject]
    public void Construct(
        BankManager bankManager,
        PlayerModel player,
        BankTransferService transferService,
        PlayerDataManager dataManager,
        GameManager gameManager)
    {
        _bankManager = bankManager;
        _player = player;
        _transferService = transferService;
        _dataManager = dataManager;
        _gameManager = gameManager;
    }

    private void Start()
    {
        GenerateBankButtons();

        singleCardPanel.SetActive(false);
        transferPanel.SetActive(false);
        withdrawPanel.SetActive(false);
        depositPanel.SetActive(false);

        transferButton.onClick.AddListener(OnTransferButtonClicked);
        confirmTransferButton.onClick.AddListener(OnConfirmTransfer);
        cancelTransferButton.onClick.AddListener(() => transferPanel.SetActive(false));
        closeSingleCardPanelButton.onClick.AddListener(() => singleCardPanel.SetActive(false));
        toCardInput.onValueChanged.AddListener(OnCardNumberChanged);

        withdrawButton.onClick.AddListener(OnWithdrawClicked);
        depositButton.onClick.AddListener(OnDepositClicked);

        confirmWithdrawButton.onClick.AddListener(OnConfirmWithdraw);
        cancelWithdrawButton.onClick.AddListener(() => withdrawPanel.SetActive(false));

        confirmDepositButton.onClick.AddListener(OnConfirmDeposit);
        cancelDepositButton.onClick.AddListener(() => depositPanel.SetActive(false));
    }

    private void GenerateBankButtons()
    {
        foreach (string bankName in _bankManager.GetBankNames())
        {
            Button newButton = Instantiate(bankButtonPrefab, bankButtonContainer);
            newButton.GetComponentInChildren<TextMeshProUGUI>().text = bankName;
            newButton.onClick.AddListener(() => OpenBank(bankName));
        }
    }

    private void OpenBank(string bankName)
    {
        _selectedBank = _bankManager.GetBank(bankName);

        var cards = _selectedBank.GetPlayerCards();
        if (cards.Count > 0)
        {
            _selectedCard = cards[0];
            cardName.text = _selectedCard.BankName;
            string last4 = "*" + _selectedCard.CardNumber.Substring(_selectedCard.CardNumber.Length - 4);
            cardNumber.text = last4;
            cardAmount.text = _selectedCard.Balance.ToString("F2") + "$";

            singleCardPanel.SetActive(true);
            transferPanel.SetActive(false);
            withdrawPanel.SetActive(false);
            depositPanel.SetActive(false);

            cardInfoText.text = $"Карта банка {_selectedBank.Name}\n" +
                                $"Номер: {_selectedCard.CardNumber}\n" +
                                $"Баланс: {_selectedCard.Balance}\n";
        }
        else
        {
            Debug.LogWarning("No card found in this bank (unexpected).");
            singleCardPanel.SetActive(false);
        }
    }

    private void OnTransferButtonClicked()
    {
        transferPanel.SetActive(true);
        toCardInput.text = "";
        amountInput.text = "";
    }

    private void OnCardNumberChanged(string input)
    {
        if (input.Length != 16)
        {
            statusText.text = "";
            return;
        }

        BankCard foundCard = _player.BankCards.Find(card => card.CardNumber == input);
        if (foundCard != null)
        {
            if (foundCard == _selectedCard)
            {
                statusText.text = "Нельзя переводить на ту же карту!";
            }
            else
            {
                statusText.text = $"Карта получателя: {foundCard.BankName}";
            }
        }
        else
        {
            statusText.text = "Такой карты не существует.";
        }
    }

    private void OnConfirmTransfer()
    {
        string fromCardNumber = _selectedCard.CardNumber;
        string toCardNumber = toCardInput.text;
        float amount;
        if (!float.TryParse(amountInput.text, out amount))
        {
            Debug.Log("Invalid amount");
            return;
        }

        bool success = _transferService.TransferMoney(fromCardNumber, toCardNumber, amount);
        if (success)
        {
            _dataManager.Save(_gameManager.PlayerModel);
            UpdateCardUI();
            Debug.Log("Transfer succeeded, data saved.");
        }
        else
        {
            Debug.Log("Transfer failed.");
        }
        transferPanel.SetActive(false);
    }


    private void OnWithdrawClicked()
    {
        withdrawPanel.SetActive(true);
        withdrawAmountInput.text = "";
    }

    private void OnConfirmWithdraw()
    {
        float amount;
        if (!float.TryParse(withdrawAmountInput.text, out amount))
        {
            Debug.LogWarning("Invalid withdraw amount");
            return;
        }

        bool success = _transferService.WithdrawToCash(_selectedCard.CardNumber, amount);
        if (success)
        {
            _dataManager.Save(_gameManager.PlayerModel);
            UpdateCardUI();
            Debug.Log("Withdraw success, data saved");
        }
        else
        {
            Debug.Log("Withdraw failed");
        }
        withdrawPanel.SetActive(false);
    }

    private void OnDepositClicked()
    {
        depositPanel.SetActive(true);
        depositAmountInput.text = "";
    }

    private void OnConfirmDeposit()
    {
        float amount;
        if (!float.TryParse(depositAmountInput.text, out amount))
        {
            Debug.LogWarning("Invalid deposit amount");
            return;
        }

        bool success = _transferService.DepositFromCash(_selectedCard.CardNumber, amount);
        if (success)
        {
            _dataManager.Save(_gameManager.PlayerModel);
            UpdateCardUI();
            Debug.Log("Deposit success, data saved");
        }
        else
        {
            Debug.Log("Deposit failed");
        }
        depositPanel.SetActive(false);
    }

    private void UpdateCardUI()
    {
        if (_selectedCard != null)
        {
            cardAmount.text = _selectedCard.Balance.ToString("F2") + "$";
            cardInfoText.text = $"Карта банка {_selectedBank.Name}\n" +
                                $"Номер: {_selectedCard.CardNumber}\n" +
                                $"Баланс: {_selectedCard.Balance}\n";
        }
    }
}
