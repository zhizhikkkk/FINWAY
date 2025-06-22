using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Animator), typeof(SpriteRenderer))]
public class PlayerAnimationController : MonoBehaviour
{
    [Header("—сылки")]
    [SerializeField] private NavMeshAgent _agent;
    private Animator _anim;
    private SpriteRenderer _sr;

    private static readonly int IsWalkingHash = Animator.StringToHash("isWalking");
    private static readonly int DirXHash = Animator.StringToHash("dirX");
    private static readonly int DirYHash = Animator.StringToHash("dirY");

    private float lastDirX = 0f;
    private float lastDirY = -1f;  

    void Awake()
    {
        _anim = GetComponent<Animator>();
        _sr = GetComponent<SpriteRenderer>();

        if (_agent == null)
            Debug.LogError("PlayerAnimationController: нет NavMeshAgent!", this);
    }

    void Update()
    {
        if (_agent == null) return;

        Vector3 vel = _agent.velocity;
        bool walking = vel.sqrMagnitude > 0.01f;

        _anim.SetBool(IsWalkingHash, walking);

        float dirX = 0f, dirY = 0f;
        if (walking)
        {
            Vector2 vn = new Vector2(vel.x, vel.y).normalized;
            dirX = Mathf.Abs(vn.x) > 0.1f ? Mathf.Sign(vn.x) : 0f;
            dirY = Mathf.Abs(vn.y) > 0.1f ? Mathf.Sign(vn.y) : 0f;

            if (Mathf.Abs(vn.y) > Mathf.Abs(vn.x))
                dirX = 0f;
            else
                dirY = 0f;

            if (dirX != 0f || dirY != 0f)
            {
                lastDirX = dirX;
                lastDirY = dirY;
            }

            if (dirX != 0f)
                _sr.flipX = dirX > 0f;
        }
        else
        {
            dirX = lastDirX;
            dirY = lastDirY;
        }

        _anim.SetFloat(DirXHash, dirX);
        _anim.SetFloat(DirYHash, dirY);
    }
}
