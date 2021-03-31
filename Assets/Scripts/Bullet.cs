using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;
    public float damage;

    Rigidbody2D _rb;

    bool _isDead;

    #region Awake
    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();   
    }
    #endregion

    #region OnEnable OnBecameInvisible
    private void OnEnable()
    {
        _rb.velocity = transform.up * speed;
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
    #endregion

    #region OnCollision
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Health health = collision.gameObject.GetComponent<Health>();
        if (health)
            health.Hit(damage);

        Destroy(gameObject);
    }
    #endregion
}
