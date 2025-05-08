using UnityEngine;

public abstract class IMovingTrap : BaseTrap
{
    [Header("Movement Settings")]
    [SerializeField] protected Transform leftPos;
    [SerializeField] protected Transform rightPos;
    [SerializeField] protected float moveSpeed = 2f;
    protected float moveDirection;
    protected float leftLimit, rightLimit;

    protected virtual void Start()
    {
        moveDirection = Mathf.Sign(transform.localScale.x);
        leftLimit = leftPos.position.x;
        rightLimit = rightPos.position.x;
    }
    protected virtual void Update()
    {
        Move();
        CheckAndFlipDirection();
    }
    protected void Move()
    {
        transform.Translate(Vector2.right * moveDirection * moveSpeed * Time.deltaTime);
    }
    protected void CheckAndFlipDirection()
    {
        if (IsBeyondLimit())
        {
            FlipDirection();
        }
    }
    protected bool IsBeyondLimit()
    {
        float posX = transform.position.x;
        return (posX <= leftLimit && moveDirection < 0) ||
                   (posX >= rightLimit && moveDirection > 0);
    }
    protected void FlipDirection()
    {
        moveDirection *= -1;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}
