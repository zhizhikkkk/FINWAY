using UnityEngine;
using Zenject;

public class HomeManager : MonoBehaviour
{
    public SleepManager sleepManager;
    public TVManager tvManager;
    [Inject] SignalBus _bus;

    void OnEnable() => _bus.Subscribe<AgentInteractionSignal>(OnInteract);
    void OnDisable() => _bus.Unsubscribe<AgentInteractionSignal>(OnInteract);

    void OnInteract(AgentInteractionSignal sig)
    {
        switch (sig.Target.tag)
        {
            case "TV": OpenTvMenu(); break;
            case "Bed": StartSleeping(); break;
        }
    }

    void OpenTvMenu() 
    {
        Debug.Log("TV");
    }
    void StartSleeping()
    {
        Debug.Log("Sleep");
        //sleepManager.ToggleSleep();
    }
}
