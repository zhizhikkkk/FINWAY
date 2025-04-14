using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using Zenject;
using Zenject.SpaceFighter;

public class BankSelectionUI : MonoBehaviour
{
    [Header("UI References")]
    public Transform bankButtonContainer;
    public Button bankButtonPrefab;

    // Панель, где показываем информацию о карте и кнопку "Перевести"
    public GameObject singleCardPanel;
    public TextMeshProUGUI cardInfoText;
    public Button transferButton;
    public Button closeSingleCardPanelButton;

    // Панель перевода (откуда -> куда -> сколько)
    public GameObject transferPanel;
    public TextMeshProUGUI cardName;
    public TextMeshProUGUI cardNumber;
    public TextMeshProUGUI cardAmount;
    public TextMeshProUGUI statusText;
    public TMP_InputField toCardInput;
    public TMP_InputField amountInput;
    public Button confirmTransferButton;
    public Button cancelTransferButton;

    private BankManager _bankManager;
    private PlayerModel _player;
    private BankTransferService _transferService;
    private Bank _selectedBank;
    private BankCard _selectedCard;
    private GameManager _gameManager;

    private PlayerDataManager _dataManager; // Чтобы сохранять JSON

    [Inject]
    public void Construct(
        BankManager bankManager,
        PlayerModel player,
        BankTransferService transferService,
        PlayerDataManager dataManager, 
        GameManager gameManager ) 
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

        // Настраиваем кнопки перевода
        transferButton.onClick.AddListener(OnTransferButtonClicked);
        confirmTransferButton.onClick.AddListener(OnConfirmTransfer);
        cancelTransferButton.onClick.AddListener(() => transferPanel.SetActive(false));
        closeSingleCardPanelButton.onClick.AddListener(() => singleCardPanel.SetActive(false));
        toCardInput.onValueChanged.AddListener(OnCardNumberChanged);
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
       

        // Так как у нас ровно 1 карта на банк, достаём её
        var cards = _selectedBank.GetPlayerCards();
        if (cards.Count > 0)
        {
            _selectedCard = cards[0];
            cardName.text = _selectedCard.BankName;
            string last4 = "*" + _selectedCard.CardNumber.Substring(_selectedCard.CardNumber.Length - 4);
            cardNumber.text = last4;
            cardAmount.text = _selectedCard.Balance.ToString() + "$";

            singleCardPanel.SetActive(true);
            transferPanel.SetActive(false);

            // Обновляем UI
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
        // Открываем панель перевода
        transferPanel.SetActive(true);
        toCardInput.text = "";
        amountInput.text = "";
    }
    private void OnCardNumberChanged(string input)
    {
        // Если длина не 16, просто очищаем статус
        if (input.Length != 16)
        {
            statusText.text = "";
            return;
        }

        // Ищем карту в списке всех карт игрока
        BankCard foundCard = _player.BankCards.Find(card => card.CardNumber == input);

        if (foundCard != null)
        {
            // Проверяем, не совпадает ли она с исходной картой
            if (foundCard == _selectedCard)
            {
                statusText.text = "Нельзя переводить на ту же карту!";
            }
            else
            {
                // Выводим краткую информацию о карте-получателе
                statusText.text = $"Карта получателя: {foundCard.BankName}";
            }
        }
        else
        {
            statusText.text = "Такой карты не существует. Проверьте номер карты.";
        }
    }

    private void OnConfirmTransfer()
    {
        string fromCardNumber = _selectedCard.CardNumber;
        string toCardNumber = toCardInput.text;
        float amount = 0f;

        if (!float.TryParse(amountInput.text, out amount))
        {
            Debug.Log("Invalid amount");
            return;
        }
        

        bool success = _transferService.TransferMoney(fromCardNumber, toCardNumber, amount);
        if (success)
        {
            // Успешный перевод → Сохраняем данные
            _dataManager.Save(_gameManager.PlayerModel);

            // Обновляем UI
            cardInfoText.text = $"Карта банка {_selectedBank.Name}\n" +
                                $"Номер: {_selectedCard.CardNumber}\n" +
                                $"Баланс: {_selectedCard.Balance}\n";
            Debug.Log("Transfer succeeded, data saved.");
        }
        else
        {
            Debug.Log("Transfer failed.");
        }

        transferPanel.SetActive(false);
    }

    

}
