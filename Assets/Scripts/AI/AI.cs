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

    [Header("Доп данные")]
    [SerializeField] LayerMask whoToTell;
    public Transform guardTarget;
    
    [Header("Проверочные данные")]
    public States currentState;
    [SerializeField] Transform attackTarget;
    [HideInInspector] public Vector2 direction;
    public float distance;

    #region Start Update
    private void OnEnable()
    {
        EnableObject(true);
        ChangeStage(startState);
    }

    private void Update()
    {
        if (attackTarget)
        {
            UpdateTargetParam();

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
        if (!walk.trackingInfinityly || !attackTarget)
        {
            this.enabled = value;
            walk.Enable(value);
        }

        attack.Enable(value);
        shot.enabled = value;

        if(!attackTarget || !value)
            senceOrgans.SetActive(value);

        body.enabled = value;
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
    public Transform GetAttackTarget() => attackTarget;

    void UpdateTargetParam()
    {
        direction = attackTarget.position - transform.position;
        distance = direction.magnitude;
    }

    public void SetTarget(Transform target, bool playSound = false, bool trackingInfinityly = false, bool tellSomeone = false)
    {
        attackTarget = target;
        UpdateTargetParam();
        ChangeStage(States.WalkToAttack);
        senceOrgans.SetActive(false);

        // playSound
        if (playSound && findTarget.Count > 0 && Random.value > 0.5f)
            audioSource.PlayOneShot(findTarget[Random.Range(0, findTarget.Count)]);

        // trackingInfinityly не должен слетать если изначально установлено всегда преследовать
        walk.trackingInfinityly = trackingInfinityly || walk.trackingInfinityly;

        // tellSomeone
        if (!tellSomeone) return;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 10f, whoToTell);
            
        for (int i = 0; i < colliders.Length; i++)
        {
            if (TryGetComponent(out AI ai))
                if (!ai.attackTarget)
                    ai.SetTarget(target);
        }
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
        attackTarget = null;
        EnableObject(false);

        Vector3 temp = transform.position;
        temp.z += 0.1f;
        transform.position = temp;
    }

    // запускается в анимации
    void DestroyObject() => Lean.Pool.LeanPool.Despawn(gameObject);
    #endregion
}
