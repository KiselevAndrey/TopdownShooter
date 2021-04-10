using UnityEngine;
using Pathfinding;
using System.Collections;

public class AIWalk : MonoBehaviour
{
    [Header("Скрипты")]
    [SerializeField] AI ai;
    [SerializeField] AIPath aiPath;
    [SerializeField] AIDestinationSetter aiDestSetter;

    [Header("Скорости")]
    [SerializeField, Range(0, 10)] float minSpeed;
    [SerializeField, Range(0, 50)] float maxSpeed;

    [Header("Дистанции")]
    public bool trackingInfinityly;
    public float maxTrackingDistance;
    public float patrolDistance;
    public float guardDistance;

    [Header("Доп данные")]
    [SerializeField] bool shotPriority;

    bool _walking;

    #region OnEnable Update
    private void OnEnable()
    {
        aiPath.maxSpeed = Random.Range(minSpeed, maxSpeed);
        _walking = true;
    }

    private void Update()
    {
        switch (ai.currentState)
        {
            case States.Guard:
            case States.Patrol:
                if(aiPath.velocity.magnitude == 0 && _walking)
                {
                    _walking = false;
                    ai.anim.SetFloat(AnimParam.Speed, 0);
                    ChangeState();
                }
                break;

            case States.WalkToAttack:
                if (shotPriority ? ai.shot.CanShoot() : ai.attack.CanAttack())
                {
                    ai.anim.SetFloat(AnimParam.Speed, 0);
                    ChangeState();
                }
                break;
        }
    }
    #endregion

    #region On Enable Disable
    public void Enable(bool value)
    {
        this.enabled = value;
        aiPath.enabled = value;
        aiDestSetter.enabled = value;
    }
    #endregion

    #region Walk
    void StartWalk()
    {
        _walking = true;

        ChangeEndReachedDistance();

        ai.anim.SetFloat(AnimParam.Speed, 1);
    }
    public void StartWalk(Vector2 target)
    {
        aiDestSetter.target = null;
        aiDestSetter.targetPosition = target;

        StartWalk();
    }

    public void StartWalk(Transform target)
    {
        aiDestSetter.target = target;

        StartWalk();
    }

    // изменяю расстояние до остановки от конечной точки
    void ChangeEndReachedDistance()
    {
        switch (ai.currentState)
        {
            case States.Guard:
                aiPath.endReachedDistance = 0.1f;
                break;

            case States.Patrol:
                aiPath.endReachedDistance = 0.1f;
                break;

            case States.WalkToAttack:
                aiPath.endReachedDistance = (shotPriority ? ai.shot.distance : ai.attack.distanceForAttack) * Random.Range(0.9f, 1.1f);
                break;
        }
    }
    #endregion

    void ChangeState()
    {
        switch (ai.currentState)
        {
            case States.Guard:
                if (ai.guardTarget)
                    StartCoroutine(ChangeStageButWaitNow(States.Guard, States.Guard, 5f));
                else
                    ai.ChangeStage(States.Idle);
                break;

            case States.Patrol:
                if(HelperBool.RandomBool())
                    StartCoroutine(ChangeStageButWaitNow(States.Patrol, States.Patrol, 5f));
                else
                    ai.ChangeStage(States.Idle);
                break;

            case States.WalkToAttack:
                if (shotPriority && ai.shot.CanShoot())
                {
                    ai.ChangeStage(States.Shot);
                }
                else if (ai.attack.CanAttack())
                    ai.ChangeStage(States.Attack);
                else
                    ai.ChangeStage(States.WalkToAttack);
                break;
        }
    }

    IEnumerator ChangeStageButWaitNow(States currentState, States newState, float waitSeconds)
    {
        yield return new WaitForSeconds(waitSeconds);
        if (ai.currentState == currentState)
            ai.ChangeStage(newState);
    }
}
