using UnityEngine;

// если есть таргет, то идти к нему
// Если дошел до мин радиаса атаки, то остановиться
// если цель ушла из макс радиуса атаки, то идти за целью
// если таргет далеко, то сделать ссылку на потерю таргета

public class Enemy : MonoBehaviour
{
    [Header("Скрипты")]
    public EnemyWalk walk;
    public EnemyShot shot;
    public EnemyAttack attack;

    [Header("Основные объекты")]
    public Animator anim;

    [HideInInspector] public Vector2 direction;
    [HideInInspector] public Transform target;

    Health _health;

    bool _isDie;


    #region Awake Start Update
    void Awake()
    {
        _health = GetComponent<Health>();
    }

    private void Start()
    {
    }

    void Update()
    {
        if (IsDead())
        {
            Die();
            return;
        }
        if (!target) return;

        direction = target.position - transform.position;
    }
    #endregion

    public bool IsDead() => _health.IsDead();
    
    void Die()
    {
        if (_isDie) return;
        
        _isDie = true;
        walk.DontMove();
    }


    #region OnCollision
    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case TagsNames.Bullet:
                Bullet bull = collision.gameObject.GetComponent<Bullet>();
                _health.Hit(bull.damage);
                Destroy(bull.gameObject);
                break;
        }
    }
    #endregion

    #region OnTrigger
    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case TagsNames.Player:
                target = FindObjectOfType<Player>().GetComponent<Transform>();
                break;
        }
    }
    #endregion
}
