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

    [HideInInspector] public States currentState;
    public Transform target;
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
    void UpdateState()
    {
        switch (currentState)
        {
            case States.Guard:
                break;
            case States.Patrol:
                break;
            case States.WalkToAttack:
                break;
            case States.Attack:
                break;
            case States.Shot:
                break;
            case States.Idle:
                break;
        }
    }

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
                break;
        }
    }

    void Walk()
    {
        walk.enabled = true;

        switch (currentState)
        {
            case States.Guard:
                walk.StartWalk(HelperVector.NewPointFromRange(target.position, walk.guardDistance));
                break;

            case States.Patrol:
                walk.StartWalk(HelperVector.NewPointFromRange(transform.position, walk.patrolDistance));
                break;

            case States.WalkToAttack:
                walk.StartWalk(target);
                break;
        }
    }

    void Attack()
    {
        walk.enabled = false;

        switch (currentState)
        {
            case States.Attack:
                break;
            case States.Shot:
                print("Shot");
                break;
        }
    }
    #endregion

    #region Target
    public void SetTarget(Transform target)
    {

    }
    #endregion
}
