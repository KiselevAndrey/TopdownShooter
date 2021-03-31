using UnityEngine;

public enum States { Guard, Patrol, Walk, Attack, Shot}
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


    [HideInInspector] public States currentState;
    [HideInInspector] public Vector2 direction;
    [HideInInspector] public Transform target;
    [HideInInspector] public float distance;
    [HideInInspector] public bool isActive;

    Health _health;

    bool _isDie;


    #region Awake Start Update
    void Awake()
    {
        _health = GetComponent<Health>();
    }

    private void Start()
    {
        currentState = canPatrol ? States.Patrol : States.Guard;
        EnableObject(false);
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
        distance = direction.magnitude;
        //CheckStatus();

        // всегда поворачиваешься к цели, если она есть
        walk.Rotation();
    }
    #endregion

    #region States
    void CheckStatus()
    {

        switch (currentState)
        {
            case States.Guard:
                break;
            case States.Patrol:
                break;
            case States.Walk:
                walk.Folloving();
                break;
            case States.Attack:
                break;
            case States.Shot:
                break;
            default:
                break;
        }
    }
    #endregion

    #region видимость игрока райкастом
    bool CanSeePlayer()
    {
        // пока просто кидаю райкаст в таргет
        LayerMask layer = LayerMask.GetMask(LayersNames.Human);
        RaycastHit2D hit2D = Physics2D.Raycast(transform.position, direction, distance, layer);
        Debug.DrawRay(transform.position, direction);
        print(hit2D.collider);
        bool temp = hit2D.collider.gameObject.CompareTag(TagsNames.Player);
        print(hit2D.collider);
        return temp;
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

    #region LoseTarget
    public void LoseTarget()
    {
        target = null;
        senceOrgans.SetActive(true);
    }
    #endregion

    #region Активность бота
    void EnableObject(bool value)
    {
        isActive = value;
        walk.enabled = value;
        shot.enabled = value;
        attack.enabled = value;
        senceOrgans.SetActive(value);
    }
    #endregion

    #region Обработка триггера органов чувств. Пока одна на всех
    public void TriggerTreat(Collider2D collision)
    {
        //switch (collision.tag)
        //{
        //    case TagsNames.Player:
        //        FindPlayer(collision);
        //        break;

        //    case TagsNames.Bullet:
        //        transform.up = Vector2.Lerp(transform.up, collision.transform.position, 1);
        //        break;
        //}
    }

    void FindPlayer(Collider2D collision)
    {
        target = collision.GetComponent<Transform>();
        direction = target.position - transform.position;
        distance = direction.magnitude;

        // не работает, устал исправлять
        if (CanSeePlayer())
        {
            print(direction);
            Debug.DrawRay(transform.position, direction);
            //senceOrgans.SetActive(false);
        }
        else target = null;
    }
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


    #region OnBecame Visible/Invisible
    private void OnBecameVisible()
    {
        EnableObject(true);
    }

    private void OnBecameInvisible()
    {
        EnableObject(false);
    }
    #endregion
}
