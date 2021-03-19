using UnityEngine;

public class Enemy : MonoBehaviour
{
    EnemyShot shot;
    EnemyWalk walk;
    Health health;

    #region Awake Start Update
    void Awake()
    {
        health = GetComponent<Health>();
    }

    private void Start()
    {
    }

    void Update()
    {
    }
    #endregion

    public bool IsDead() => health.IsDead();


    #region OnCillision
    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case TagsNames.Bullet:
                Bullet bull = collision.gameObject.GetComponent<Bullet>();
                health.Hit(bull.damage);
                Destroy(bull.gameObject);
                break;
        }
    }
    #endregion
}
