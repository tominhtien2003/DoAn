using UnityEngine;

public class BasePlayer : MonoBehaviour
{
    public Animator playerAnimator;
    public Rigidbody2D rb;

    protected Vector2 moveDirection;


    [SerializeField] protected Transform playerGFX;
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected float runSpeed;
    [SerializeField] protected float jumpImpulse;
    [SerializeField] protected float airSpeed;

    public virtual void Awake()
    {
        playerAnimator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }
}
