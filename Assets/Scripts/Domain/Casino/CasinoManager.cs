using UnityEngine;
using Zenject;

public class CasinoManager : MonoBehaviour
{
    [Inject] private SignalBus _bus;
    [Inject] private PlayerModel _playerModel;

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

    private void OnInteract(AgentInteractionSignal sig)
    {
        Debug.Log("Slot");
        if (sig.Target.CompareTag("SlotMachine"))
        {
            Debug.Log("Slot");
            var machine = sig.Target.GetComponent<SlotMachine>();
            if (machine != null)
                machine.StartGame(_playerModel);
        }
    }

    private void OnNewTarget(GameObject target)
    {
        if (target == null || !target.CompareTag("SlotMachine"))
            SlotMachine.CloseAnyOpenUI();
    }
}
