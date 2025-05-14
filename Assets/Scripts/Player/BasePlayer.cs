using UnityEngine;

public class BasePlayer : MonoBehaviour
{
    public Animator playerAnimator;
    public Rigidbody2D rb;

    protected Vector2 moveDirection;


    [SerializeField] protected Transform playerGFX;
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected float jumpImpulse;

    public virtual void Awake()
    {
        playerAnimator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }
}
