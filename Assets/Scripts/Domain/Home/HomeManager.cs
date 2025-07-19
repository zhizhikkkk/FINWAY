using UnityEngine;
using Zenject;

public class HomeManager : MonoBehaviour
{
    [SerializeField] SleepManager sleepManager;
    [SerializeField] TVManager tvManager;

    [Inject] SignalBus _bus;

    void OnEnable()
    {
        _bus.Subscribe<AgentInteractionSignal>(OnInteract);
        AgentMovement.OnNewTarget += OnNewTarget;
    }

    void OnDisable()
    {
        _bus.Unsubscribe<AgentInteractionSignal>(OnInteract);
        AgentMovement.OnNewTarget -= OnNewTarget;
    }

    void OnInteract(AgentInteractionSignal sig)
    {
        Debug.Log("Home");
        switch (sig.Target.tag)
        {
            case "TV":
                tvManager.StartWatching(sig.Agent.transform);
                break;

            case "Bed":
                Debug.Log("Salam");
                sleepManager.StartSleeping(sig.Agent.transform);
                break;
        }
    }
    void OnNewTarget(GameObject target)
    {
        if (tvManager.IsWatching && (target == null || target.tag != "TV"))
            tvManager.StopWatching();
    }
}
