using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float speed;

    public int damage;

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
}
