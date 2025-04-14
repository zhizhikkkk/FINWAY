using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using Zenject;

public class TVManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Button toggleWatchButton;
    [SerializeField] private TextMeshProUGUI buttonText;

    [Header("Настройки просмотра")]
    [SerializeField] private float happinessPerHour = 2f;
    [SerializeField] private float energyPerHour = 1f;

    private bool isWatching = false;
    private Coroutine watchRoutine;
    private PlayerModel playerModel;

    [Inject]
    public void Construct(PlayerModel model)
    {
        playerModel = model;
    }

    private void Start()
    {
        toggleWatchButton.onClick.AddListener(ToggleWatch);
        UpdateButtonText();
    }

    private void ToggleWatch()
    {
        isWatching = !isWatching;

        if (isWatching)
        {
            watchRoutine = StartCoroutine(WatchTV());
        }
        else
        {
            StopWatching();
        }

        UpdateButtonText();
    }

    private IEnumerator WatchTV()
    {
        float timer = 0f;

        while (isWatching)
        {
            timer += Time.deltaTime;

            if (timer >= 1f)
            {
                int hours = Mathf.FloorToInt(timer);
                timer -= hours;

                for (int i = 0; i < hours; i++)
                {
                    playerModel.AddHours(1);
                    playerModel.ChangeHappiness(happinessPerHour);
                    playerModel.ChangeHappiness(energyPerHour);
                }
            }

            yield return null;
        }
    }

    private void StopWatching()
    {
        isWatching = false;

        if (watchRoutine != null)
        {
            StopCoroutine(watchRoutine);
            watchRoutine = null;
        }

        UpdateButtonText();
    }

    private void UpdateButtonText()
    {
        buttonText.text = isWatching ? "Stop Watching" : "Watch TV";
    }
}
