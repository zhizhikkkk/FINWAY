using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using Zenject;

[RequireComponent(typeof(NavMeshAgent))]
public class AgentMovement : MonoBehaviour
{
    /* ---------- ��������� ---------- */
    [Header("Input / Camera")]
    [SerializeField] InputAction click;
    [SerializeField] Camera cam;

    [Header("Interaction")]
    [SerializeField] LayerMask interactMask;          // Interactable ����
    [SerializeField] float arriveDist = 0.35f;    // ������� �������
    [SerializeField] float sampleRadius = 0.8f;     // NavMesh.SamplePosition
    [SerializeField] float minPathToArm = 0.25f;    


    /* ---------- ���� ---------- */
    NavMeshAgent agent;
    GameObject interactObj;
    bool awaitingInteraction;   // ��� ��
    bool armed;                 // ���� ��� �����������

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

        /* ��� ������� --------------------------------------------- */
        if (awaitingInteraction &&
            !agent.pathPending &&
            agent.pathStatus == NavMeshPathStatus.PathComplete)
        {
            /* ���� ��� �������� (���������� �������) � ������ �� � ���� */
            if (armed &&
                agent.remainingDistance <= Mathf.Max(arriveDist, agent.stoppingDistance))
            {
                _bus.Fire(new AgentInteractionSignal(gameObject, interactObj));

                awaitingInteraction = armed = false;
                interactObj = null;
            }
        }
    }

    /* ---------- ��������� ����� ---------- */
    void HandleClick()
    {
        /* �����  ��� ------------------------------------------------- */
        Vector3 mouse = Mouse.current.position.ReadValue();
        mouse.z = -cam.transform.position.z;
        Vector3 world = cam.ScreenToWorldPoint(mouse); world.z = 0;

        /* ����������? ------------------------------------------------- */
        Collider2D hit = Physics2D.OverlapPoint(world, interactMask);

        /* ��������� ����� ��� ������� -------------------------------- */
        if (hit)
        {
            interactObj = hit.gameObject;
            world = hit.ClosestPoint(world);     // ����� �������� ������
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

            /* ����������, ���������� �� ���������� (���� > minPathToArm) */
            float pathLen = Vector3.Distance(transform.position, navHit.position);
            armed = awaitingInteraction && pathLen >= minPathToArm;
        }
    }
}
