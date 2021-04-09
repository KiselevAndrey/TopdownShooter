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
    }

    private void Update()
    {
        if (attackTarget)
        {
            direction = attackTarget.position - transform.position;
            distance = direction.magnitude;

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
        anim.enabled = value;
        walk.enabled = value;
        shot.enabled = value;
        attack.enabled = value;
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
                print("Guard");
                Walk();
                break;
            case States.Patrol:
                print("Patrol");
                Walk();
                break;
            case States.WalkToAttack:
                print("Walk");
                Walk();
                break;
            case States.Attack:
                Attack();
                break;
            case States.Shot:
                Attack();
                break;
            case States.Idle:
                print("Idle");
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
                walk.StartWalk(HelperVector.NewPointFromRange(attackTarget.position, walk.guardDistance));
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
                print("Attack");
                attack.Attack();
                break;
            case States.Shot:
                print("Shot");
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
    public void SetTarget(Transform target)
    {
        attackTarget = target;
        ChangeStage(States.WalkToAttack);
        senceOrgans.SetActive(false);
    }

    public void LoseTarget()
    {
        attackTarget = null;
        ChangeStage(States.Idle);
        senceOrgans.SetActive(true);
    }
    #endregion
}
