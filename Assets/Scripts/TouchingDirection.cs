using UnityEngine;

public class TouchingDirection : MonoBehaviour
{
    [SerializeField] private CapsuleCollider2D playerCollider;
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private ContactFilter2D castFilter;
    [SerializeField] private float groundDistance = .05f;
    [SerializeField] private float wallDistance = .2f;
    [SerializeField] private float ceilingDistance = .05f;

    private RaycastHit2D[] groundHits = new RaycastHit2D[5];
    private RaycastHit2D[] wallHits = new RaycastHit2D[5];
    private RaycastHit2D[] ceilingHits = new RaycastHit2D[5];

    private Vector2 checkDirectionPlayer => transform.localScale.x > 0 ? Vector2.right : Vector2.left;

    private bool isGround;
    public bool IsGround 
    {
        get
        {
            return isGround;
        }
        private set
        {
            isGround = value;
            playerAnimator.SetBool(AnimationUtilities.IS_GROUND, value);
        }
    }
    private bool isWall;

    public bool IsWall
    {
        get
        {
            return isWall;
        }
        private set
        {
            isWall = value;
            playerAnimator.SetBool(AnimationUtilities.IS_WALL, value);
        }
    }
    private bool isCeiling;

    public bool IsCeiling
    {
        get
        {
            return isCeiling;
        }
        private set
        {
            isCeiling = value;
            playerAnimator.SetBool(AnimationUtilities.IS_CEILING, value);
        }
    }
    private void FixedUpdate()
    {
        IsGround = playerCollider.Cast(Vector2.down, castFilter, groundHits, groundDistance) > 0;
        IsWall = playerCollider.Cast(checkDirectionPlayer, castFilter, wallHits, wallDistance) > 0;
        IsCeiling = playerCollider.Cast(Vector2.up, castFilter, ceilingHits, ceilingDistance) > 0;
    }
}
