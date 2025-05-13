using UnityEngine;

public class CollectibleItem : MonoBehaviour
{
    [SerializeField] private int value; // Giá trị của vật phẩm (ví dụ: số coin, số máu hồi)
    [SerializeField] private ItemType itemType; // Loại vật phẩm
    [SerializeField] private float magnetRadius = 2f; // Bán kính hút vật phẩm về phía player
    [SerializeField] private float magnetSpeed = 10f; // Tốc độ hút
    [SerializeField] private LayerMask groundLayer; // Layer của mặt đất
    [SerializeField] private float groundCheckDistance = 0.1f; // Khoảng cách kiểm tra mặt đất

    private bool isBeingMagneted = false;
    private bool isGrounded = false;
    private Transform playerTransform;
    private Rigidbody2D rb;
    private CircleCollider2D itemCollider;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        itemCollider = GetComponent<CircleCollider2D>();
        
        // Thiết lập Rigidbody2D
        if (rb != null)
        {
            rb.gravityScale = 1f;
            rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
            rb.freezeRotation = true; // Khóa xoay
        }
    }

    private void Update()
    {
        // Kiểm tra xem vật phẩm có chạm đất không
        CheckGrounded();

        // Nếu đã chạm đất, tắt gravity và khóa vị trí
        if (isGrounded && rb != null)
        {
            rb.gravityScale = 0f;
            rb.linearVelocity = Vector2.zero;
        }

        // Xử lý hiệu ứng hút vật phẩm
        if (isBeingMagneted && playerTransform != null)
        {
            Vector2 direction = (playerTransform.position - transform.position).normalized;
            transform.position = Vector2.MoveTowards(
                transform.position,
                playerTransform.position,
                magnetSpeed * Time.deltaTime
            );
        }
    }

    private void CheckGrounded()
    {
        if (isGrounded) return;

        // Tạo raycast xuống dưới để kiểm tra mặt đất
        RaycastHit2D hit = Physics2D.Raycast(
            transform.position,
            Vector2.down,
            groundCheckDistance,
            groundLayer
        );

        if (hit.collider != null)
        {
            isGrounded = true;
            // Điều chỉnh vị trí để vật phẩm nằm trên mặt đất
            transform.position = new Vector3(
                transform.position.x,
                hit.point.y + itemCollider.radius,
                transform.position.z
            );
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Collect(other.gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        // Chỉ hút vật phẩm khi đã chạm đất
        if (other.CompareTag("Player") && !isBeingMagneted && isGrounded)
        {
            float distance = Vector2.Distance(transform.position, other.transform.position);
            if (distance <= magnetRadius)
            {
                isBeingMagneted = true;
                playerTransform = other.transform;
            }
        }
    }

    private void Collect(GameObject player)
    {
        switch (itemType)
        {
            case ItemType.Coin:
                // Thêm coin vào inventory hoặc score
                GameManager.Instance?.AddCoins(value);
                break;
            case ItemType.Blood:
                // Hồi máu cho player
                var playerHealth = player.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    playerHealth.Heal(value);
                }
                break;
        }

        // Phát âm thanh nhặt vật phẩm (nếu có)
        //AudioManager.Instance?.PlayCollectSound();

        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, magnetRadius);
        
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * groundCheckDistance);
    }
}

public enum ItemType
{
    Coin,
    Blood
} 