using UnityEngine;

public class BaseEnemy : MonoBehaviour
{
    [SerializeField] protected float walkSpeed;

    [SerializeField] protected Animator enemyAnimator;
    
    public Rigidbody2D rb { get; set; }

    public float MoveDirection { get; set; }
    public float GetWalkSpeed() => walkSpeed;
    public Animator GetEnemyAnimator() => enemyAnimator;
    public virtual void FaceDirection(float dir)
    {
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * Mathf.Sign(dir);
        transform.localScale = scale;
    }
}
