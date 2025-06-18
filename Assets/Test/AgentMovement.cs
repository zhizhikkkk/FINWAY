using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using Zenject;

[RequireComponent(typeof(NavMeshAgent))]
public class AgentMovement : MonoBehaviour
{
    public static event Action<GameObject> OnNewTarget;  

    [Header("Input / Camera")]
    [SerializeField] InputAction click;
    [SerializeField] Camera cam;

    [Header("NavMesh / Interaction")]
    [SerializeField] LayerMask interactMask;
    [SerializeField] float arriveDist = 0.35f;
    [SerializeField] float sampleRadius = 0.8f;
    [SerializeField] float minPathToArm = 0.25f;

    NavMeshAgent agent;
    GameObject interactObj;
    bool awaiting;
    bool armed;         

    [Inject] SignalBus _bus;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        if (!cam) cam = Camera.main;
    }

    void OnEnable() => click.Enable();
    void OnDisable() => click.Disable();

    void Update()
    {
        if (click.WasPressedThisFrame() &&
            !EventSystem.current.IsPointerOverGameObject(Mouse.current.deviceId))
        {
            HandleClick();
        }

        if (awaiting &&
            !agent.pathPending &&
            agent.pathStatus == NavMeshPathStatus.PathComplete &&
            armed &&
            agent.remainingDistance <= Mathf.Max(arriveDist, agent.stoppingDistance))
        {
            _bus.Fire(new AgentInteractionSignal(gameObject, interactObj));
            awaiting = armed = false;
        }
    }

    void HandleClick()
    {
        Vector3 m = Mouse.current.position.ReadValue();
        m.z = -cam.transform.position.z;
        var world = cam.ScreenToWorldPoint(m); world.z = 0;

        var hit = Physics2D.OverlapPoint(world, interactMask);
        interactObj = hit ? hit.gameObject : null;

        if (hit)                                
        {
            world = hit.ClosestPoint(world); 
            awaiting = true;
        }
        else
        {
            awaiting = false;
        }

        if (NavMesh.SamplePosition(world, out var navHit, sampleRadius, NavMesh.AllAreas))
        {
            agent.SetDestination(navHit.position);
            var pathLen = Vector3.Distance(transform.position, navHit.position);
            armed = awaiting && pathLen >= minPathToArm;
        }

        OnNewTarget?.Invoke(interactObj);      
    }
}
