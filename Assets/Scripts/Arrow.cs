using System.Collections;
using UnityEngine;

public class Arrow : MonoBehaviour , IWeaponRanged
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float damage = 15f;
    [SerializeField] private float lifetime ;
    [SerializeField] private LayerMask playerLayer;
    public float MoveDirection { get; set; }
    private Coroutine deactivateCoroutine;

    public void Deactivate()
    {

        if (deactivateCoroutine != null)
        {
            StopCoroutine(deactivateCoroutine);
            deactivateCoroutine = null;
        }
        gameObject.SetActive(false);
    }

    public void Initialize(Vector3 position, float direction)
    {
        transform.position = position;
        MoveDirection = direction;

        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * (direction < 0 ? -1 : 1);
        transform.localScale = scale;

        gameObject.SetActive(true);

        if (deactivateCoroutine != null)
        {
            StopCoroutine(deactivateCoroutine);
        }
        deactivateCoroutine = StartCoroutine(AutoDeactivate());
    }
    private IEnumerator AutoDeactivate()
    {
        yield return new WaitForSeconds(lifetime);
        Deactivate();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & playerLayer) != 0)
        {
            Debug.Log("Hit enemy: " + collision.gameObject.name);
            Transform parentTransform = collision.transform;
            if (parentTransform != null && parentTransform.TryGetComponent<IDamageable>(out var damageable))
            {
                damageable.TakeDamage(damage);
                Destroy(gameObject);
            }
        }
        else if (collision.CompareTag("Ground") || collision.CompareTag("Obstacle"))
        {
            Deactivate();
        }
    }
    private void Update()
    {
        transform.position += transform.right * MoveDirection * speed * Time.deltaTime;
    }
}
