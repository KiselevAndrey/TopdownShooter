using System.Collections.Generic;
using UnityEngine;

public enum States { Guard, Patrol, WalkToAttack, Attack, Shot, Idle }
public class AI : MonoBehaviour
{
    [Header("Описание")]
    [SerializeField, TextArea(1, 4)] string descriptions;

    [Header("Скрипты")]
    public AIWalk walk;
    public AIShot shot;
    public AIAttack attack;
    [SerializeField] Health health;

    [Header("Осн переменные")]
    [SerializeField] States startState;

    [Header("Основные объекты")]
    public Animator anim;
    [SerializeField] GameObject senceOrgans;
    [SerializeField] CircleCollider2D body;

    [Header("Звуки")]
    [SerializeField] AudioSource audioSource;
    [SerializeField] List<AudioClip> findTarget;

    public States currentState;
    public Transform attackTarget;
    public Transform guardTarget;
    [HideInInspector] public Vector2 direction;
    [HideInInspector] public float distance;
    [HideInInspector] public bool isDie;

    #region Start Update
    private void Start()
    {
        ChangeStage(startState);
        if (attackTarget) SetTargetParam();
    }

    private void Update()
    {
        if(IsDead())
        {
            Die();
            return;
        }

        if (attackTarget)
        {
            SetTargetParam();

            if (distance > walk.maxTrackingDistance && !walk.trackingInfinityly)
                LoseTarget();
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

    #region Вкл выкл скриптов
    void EnableObject(bool value)
    {
        this.enabled = value;
        walk.Enable(value);
        attack.Enable(value);
        shot.enabled = value;
        senceOrgans.SetActive(value);
    }
    #endregion

    #region States
    public void ChangeStage(States newState)
    {
        currentState = newState;

        switch (newState)
        {
            case States.Guard:
            case States.Patrol:
            case States.WalkToAttack:
                Walk();
                break;

            case States.Attack:
            case States.Shot:
                Attack();
                break;

            case States.Idle:
                Idle();
                break;
        }
    }

    void Walk()
    {
        walk.Enable(true);

        switch (currentState)
        {
            case States.Guard:
                walk.StartWalk(HelperVector.NewPointFromRange(guardTarget.position, walk.guardDistance));
                break;

            case States.Patrol:
                walk.StartWalk(HelperVector.NewPointFromRange(transform.position, walk.patrolDistance));
                break;

            case States.WalkToAttack:
                walk.StartWalk(attackTarget);
                break;
        }
    }

    void Attack()
    {
        walk.Enable(false);

        switch (currentState)
        {
            case States.Attack:
                attack.Attack();
                break;

            case States.Shot:
                break;
        }
    }

    void Idle()
    {
        if (attackTarget)
            ChangeStage(States.WalkToAttack);

        else if(guardTarget)
            ChangeStage(States.Guard);

        else
            ChangeStage(States.Patrol);
    }
    #endregion

    #region Target
    void SetTargetParam()
    {
        direction = attackTarget.position - transform.position;
        distance = direction.magnitude;
    }

    public void SetTarget(Transform target)
    {
        attackTarget = target;
        ChangeStage(States.WalkToAttack);
        senceOrgans.SetActive(false);
        SetTargetParam();
    }

    public void LoseTarget()
    {
        attackTarget = null;
        ChangeStage(States.Idle);
        senceOrgans.SetActive(true);
    }
    #endregion

    #region Die
    public bool IsDead() => health.IsDead();

    void Die()
    {
        if (isDie) return;

        isDie = true;

        walk.Enable(false);

        EnableObject(false);

        body.enabled = false;
        Vector3 temp = transform.position;
        temp.z += 0.1f;
        transform.position = temp;
    }

    void DestroyObject() => Destroy(gameObject);
    #endregion
}
