using UnityEngine;

public enum States { Idle, Patrol, Chasing, Attack, Shot}
public class Enemy : MonoBehaviour
{
    [Header("Описание")]
    [SerializeField, Multiline(3)] string descriptions;

    [Header("Скрипты")]
    public EnemyWalk walk;
    public EnemyShot shot;
    public EnemyAttack attack;

    [Header("Основные объекты")]
    public Animator anim;

    [Header("Доп объекты")]
    [SerializeField] CircleCollider2D body;
    [SerializeField] GameObject senceOrgans;

    [Header("Доп переменные")]
    [SerializeField] bool canPatrol;


    [HideInInspector] public Vector2 direction;
    [HideInInspector] public Transform target;
    [HideInInspector] public States currentState;

    Health _health;

    bool _isDie;


    #region Awake Start Update
    void Awake()
    {
        _health = GetComponent<Health>();
    }

    private void Start()
    {
        currentState = canPatrol ? States.Patrol : States.Idle;
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

    #region Die
    public bool IsDead() => _health.IsDead();
    
    void Die()
    {
        if (_isDie) return;
        
        _isDie = true;

        walk.DontMove();

        EnableObject(false);

        body.enabled = false;
        Vector3 temp = transform.position;
        temp.z += 0.1f;
        transform.position = temp;
    }

    void DestroyObject() => Destroy(gameObject);
    #endregion

    #region Доп функции 
    #region LoseTarget
    public void LoseTarget()
    {
        target = null;
        senceOrgans.SetActive(true);
    }
    #endregion

    void EnableObject(bool value)
    {
        walk.enabled = value;
        shot.enabled = value;
        attack.enabled = value;
    }

    #region Обработка триггера
    public void TriggerTreat(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case TagsNames.Player:
                target = FindObjectOfType<Player>().GetComponent<Transform>();
                direction = target.position - transform.position;
                senceOrgans.SetActive(false);
                break;

            case TagsNames.Bullet:
                transform.up = Vector2.Lerp(transform.up, collision.transform.position, 1);
                break;
        }
    }
    #endregion
    #endregion

    #region OnCollision
    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case TagsNames.Bullet:
                if (!target)
                {
                    Vector2 dir = collision.contacts[0].point - (Vector2)transform.position;
                    transform.up = dir;
                }
                break;
        }
    }
    #endregion
}
