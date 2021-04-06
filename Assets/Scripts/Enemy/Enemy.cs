using System.Collections.Generic;
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

    [Header("Звуки")]
    [SerializeField] AudioSource audioSource;
    [SerializeField] List<AudioClip> findTarget;


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

    #region Target
    public void LoseTarget()
    {
        target = null;
        senceOrgans.SetActive(true);
    }
    public void SetTarget(Transform target, bool playSound = false)
    {
        this.target = target;
        senceOrgans.SetActive(false);

        if(playSound && Random.value > 0.5f)
            audioSource.PlayOneShot(findTarget[Random.Range(0, findTarget.Count)]);

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 10f, LayerMask.GetMask(LayersNames.Zombie));
        for (int i = 0; i < colliders.Length; i++)
        {
            Enemy zombie = colliders[i].GetComponent<Enemy>();
            if (zombie && !zombie.target) zombie.SetTarget(target);
        }
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
