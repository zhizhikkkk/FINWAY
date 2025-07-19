using System.Collections;
using TMPro;
using UnityEngine;
using Zenject;
using UnityEngine.SceneManagement;

public class SleepManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] TextMeshProUGUI sleepText;

    [Header("Баланс сна")]
    [SerializeField] float happinessAdd = 10f;   
    [SerializeField] float maxEnergy=100f;

    private PlayerModel _player;

    [Inject]
    void Construct(PlayerModel model)
    {
        _player = model;
    }


    public void StartSleeping(Transform sleeper)
    {
        Debug.Log("StartSleeping");
        _player.Energy.Value = maxEnergy;
        _player.Happiness.Value += happinessAdd;
        SceneManager.LoadScene("RealLife");
    }

    
}
