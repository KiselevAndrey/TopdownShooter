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
        _isDead = false;
    }

    private void OnBecameInvisible()
    {
        Dead();
    }
    #endregion

    #region Dead & HitSelf
    void Dead()
    {
        if (_isDead) return;

        _isDead = true;
        Lean.Pool.LeanPool.Despawn(gameObject);
    }

    void HitSelf(int damage = 1)
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

        // сколько колизий сможет обработать за один кадр
        HitSelf();

        if (collision.gameObject.TryGetComponent(out Health health))
            health.Hit(damage);

        // в любом случае после соприкосновения уничтожаем
        Dead();
    }
    #endregion
}
