using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using Zenject;

public class SleepManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Button toggleSleepButton;
    [SerializeField] private TextMeshProUGUI buttonText;

    [Header("Настройки сна")]
    [SerializeField] private float energyPerHour = 2f;
    [SerializeField] private float maxEnergy = 100f;
    [SerializeField] private float happinessPerHour = 10f;

    private bool isSleeping = false;
    private Coroutine sleepRoutine;
    private PlayerModel playerModel;

    [Inject]
    public void Construct(PlayerModel model)
    {
        playerModel = model;
    }

    private void Start()
    {
        toggleSleepButton.onClick.AddListener(ToggleSleep);
        UpdateButtonText();
    }

    private void ToggleSleep()
    {
        // Не даём заснуть, если энергия уже максимальная
        if (!isSleeping && playerModel.Energy.Value >= maxEnergy)
        {
            Debug.Log("Вы уже полностью отдохнули.");
            return;
        }

        isSleeping = !isSleeping;

        if (isSleeping)
        {
            sleepRoutine = StartCoroutine(SleepLoop());
        }
        else
        {
            StopSleeping();
        }

        UpdateButtonText();
    }

    private IEnumerator SleepLoop()
    {
        float timer = 0f;

        while (isSleeping)
        {
            timer += Time.deltaTime;

            if (timer >= 1f)
            {
                int hours = Mathf.FloorToInt(timer);
                timer -= hours;

                for (int i = 0; i < hours; i++)
                {
                    if (playerModel.Energy.Value < maxEnergy)
                    {
                        playerModel.AddHours(1);
                        playerModel.ChangeEnergy(energyPerHour);
                        playerModel.ChangeHappiness(happinessPerHour);
                    }
                    else
                    {
                        Debug.Log("Энергия достигла максимума — пробуждение.");
                        StopSleeping(); // Автоматическое пробуждение
                        yield break;
                    }
                }
            }

            yield return null;
        }
    }

    private void StopSleeping()
    {
        isSleeping = false;

        if (sleepRoutine != null)
        {
            StopCoroutine(sleepRoutine);
            sleepRoutine = null;
        }

        UpdateButtonText();
    }

    private void UpdateButtonText()
    {
        buttonText.text = isSleeping ? "Wake up" : "Sleep";
    }
}
