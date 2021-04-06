using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;
    public float damage;
    public int life;

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

    #region HitSelf
    void Dead()
    {
        if (_isDead) return;

        _isDead = true;
        Destroy(gameObject);
    }

    public void GetHitSelf(int damage = 1)
    {
        life -= damage;

        if (life <= 0)
            Dead();
    }

    #endregion

    #region LifeTime
    public void SetLifeTime(float time)
    {
        StartCoroutine(Life(time));
    }

    IEnumerator Life(float time)
    {
        yield return new WaitForSeconds(time);
        Dead();
    }

    #endregion

    #region OnCollision
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (_isDead) return;

        GetHitSelf();
        Health health = collision.gameObject.GetComponent<Health>();
        if (health)
            health.Hit(damage);

        Dead();
    }
    #endregion
}
