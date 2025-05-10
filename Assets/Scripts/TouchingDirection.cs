using UnityEngine;

public class TouchingDirection : MonoBehaviour
{
    [Header("References")]
    private CapsuleCollider2D playerCollider;
    [SerializeField] private Animator playerAnimator;

    [Header("Settings")]
    [SerializeField] private ContactFilter2D castFilter;
    [SerializeField] private float groundDistance = 0.05f;
    [SerializeField] private float wallDistance = 0.2f;
    [SerializeField] private float ceilingDistance = 0.05f;

    private RaycastHit2D[] hits = new RaycastHit2D[5];

    public bool IsGround { get; private set; }
    public bool IsWall { get; private set; }
    public bool IsCeiling { get; private set; }

    private Vector2 CheckDirection => transform.localScale.x > 0 ? Vector2.right : Vector2.left;
    private void Awake()
    {
        playerCollider = GetComponent<CapsuleCollider2D>();
    }
    private void FixedUpdate()
    {
        IsGround = Check(Vector2.down, groundDistance);
        IsWall = Check(CheckDirection, wallDistance);
        IsCeiling = Check(Vector2.up, ceilingDistance);

        UpdateAnimator();
    }

    private bool Check(Vector2 direction, float distance)
    {
        return playerCollider.Cast(direction, castFilter, hits, distance) > 0;
    }

    private void UpdateAnimator()
    {
        playerAnimator.SetBool(AnimationUtilities.IS_GROUND, IsGround);
        playerAnimator.SetBool(AnimationUtilities.IS_WALL, IsWall);
        playerAnimator.SetBool(AnimationUtilities.IS_CEILING, IsCeiling);
    }
}
