using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using Zenject;

public class WorkLoopManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Button toggleButton;
    [SerializeField] private TextMeshProUGUI buttonText;
    [SerializeField] private Image progressFill;

    [Header("Настройки работы")]
    [SerializeField] private float workCycleDuration = 3f;
    [SerializeField] private int moneyPerCycle = 50;
    [SerializeField] private int energyCost = 5;
    [SerializeField] private string jobId = "office";
    private Coroutine workCoroutine;
    private bool isWorking = false;

    private PlayerModel playerModel;

    [Inject]
    public void Construct(PlayerModel model)
    {
        playerModel = model;
    }

    private void Start()
    {
        toggleButton.onClick.AddListener(ToggleWork);
        UpdateButtonText();
    }

    private void ToggleWork()
    {
        isWorking = !isWorking;

        if (isWorking)
        {
            workCoroutine = StartCoroutine(WorkLoop());
        }
        else
        {
            if (workCoroutine != null)
            {
                StopCoroutine(workCoroutine);
                workCoroutine = null;
            }

        }

        UpdateButtonText();
    }


    private IEnumerator WorkLoop()
    {
        while (isWorking)
        {
            float t = workCycleDuration * playerModel.GetWorkProgress(jobId);
            float timeAccumulator = t;

            while (t < workCycleDuration)
            {
                float delta = Time.deltaTime;
                t += delta;
                timeAccumulator += delta;

                float progress = Mathf.Clamp01(t / workCycleDuration);
                progressFill.fillAmount = progress;
                playerModel.SetWorkProgress(jobId, progress);

                if (timeAccumulator >= 1f)
                {
                    int hoursToAdd = Mathf.FloorToInt(timeAccumulator);
                    playerModel.AddHours(hoursToAdd);
                    timeAccumulator -= hoursToAdd;
                }

                yield return null;
            }

            progressFill.fillAmount = 0f;
            playerModel.SetWorkProgress(jobId, 0f);

            playerModel.Cash.Value += moneyPerCycle;
            playerModel.ChangeEnergy(-energyCost);

            if (playerModel.Energy.Value < energyCost)
            {
                ToggleWork(); // автоматически выключит работу и цикл завершится
            }
        }
    }




    private void UpdateButtonText()
    {
        buttonText.text = isWorking ? "Stop" : "Start";
    }
}
