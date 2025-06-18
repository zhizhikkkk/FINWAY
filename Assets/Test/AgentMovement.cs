using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using Zenject;

[RequireComponent(typeof(NavMeshAgent))]
public class AgentMovement : MonoBehaviour
{
    /* ---------- инспектор ---------- */
    [Header("Input / Camera")]
    [SerializeField] InputAction click;
    [SerializeField] Camera cam;

    [Header("Interaction")]
    [SerializeField] LayerMask interactMask;          // Interactable слой
    [SerializeField] float arriveDist = 0.35f;    // считаем «пришёл»
    [SerializeField] float sampleRadius = 0.8f;     // NavMesh.SamplePosition
    [SerializeField] float minPathToArm = 0.25f;    


    /* ---------- поля ---------- */
    NavMeshAgent agent;
    GameObject interactObj;
    bool awaitingInteraction;   // ждём ли
    bool armed;                 // путь был достаточный

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

    /* ---------- Update ---------- */
    void Update()
    {
        if (click.WasPressedThisFrame() &&
            !EventSystem.current.IsPointerOverGameObject(Mouse.current.deviceId))
        {
            HandleClick();
        }

        /* ждём прихода --------------------------------------------- */
        if (awaitingInteraction &&
            !agent.pathPending &&
            agent.pathStatus == NavMeshPathStatus.PathComplete)
        {
            /* Путь был «вооружён» (достаточно длинный) и теперь мы у цели */
            if (armed &&
                agent.remainingDistance <= Mathf.Max(arriveDist, agent.stoppingDistance))
            {
                _bus.Fire(new AgentInteractionSignal(gameObject, interactObj));

                awaitingInteraction = armed = false;
                interactObj = null;
            }
        }
    }

    /* ---------- обработка клика ---------- */
    void HandleClick()
    {
        /* экран  мир ------------------------------------------------- */
        Vector3 mouse = Mouse.current.position.ReadValue();
        mouse.z = -cam.transform.position.z;
        Vector3 world = cam.ScreenToWorldPoint(mouse); world.z = 0;

        /* интерактив? ------------------------------------------------- */
        Collider2D hit = Physics2D.OverlapPoint(world, interactMask);

        /* ближайшая точка для навмеша -------------------------------- */
        if (hit)
        {
            interactObj = hit.gameObject;
            world = hit.ClosestPoint(world);     // может остаться внутри
            awaitingInteraction = true;
        }
        else
        {
            interactObj = null;
            awaitingInteraction = false;
        }

        if (NavMesh.SamplePosition(world, out var navHit, sampleRadius, NavMesh.AllAreas))
        {
            agent.SetDestination(navHit.position);

            /* определяем, «вооружена» ли интеракция (путь > minPathToArm) */
            float pathLen = Vector3.Distance(transform.position, navHit.position);
            armed = awaitingInteraction && pathLen >= minPathToArm;
        }
    }
}
