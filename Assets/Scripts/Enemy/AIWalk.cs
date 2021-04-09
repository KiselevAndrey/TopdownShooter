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

    #region Start Update
    private void Start()
    {
        aiPath.maxSpeed = Random.Range(minSpeed, maxSpeed);
    }

    private void Update()
    {
        if (shotPriority ? ai.shot.CanShoot() : ai.attack.CanAttack())
        {
            ai.anim.SetFloat(AnimParam.Speed, 0);
            ChangeState();
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
    public void StartWalk(Vector2 target)
    {
        ChangeEndReachedDistance();

        aiDestSetter.target = null;
        aiDestSetter.targetPosition = target;

        ai.anim.SetFloat(AnimParam.Speed, 1);
    }

    public void StartWalk(Transform target)
    {
        ChangeEndReachedDistance();

        aiDestSetter.target = target;

        ai.anim.SetFloat(AnimParam.Speed, 1);
    }

    // изменяю расстояние до остановки от игрока
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
                StartCoroutine(ChangeStageButWaitNow(States.Guard, States.Guard, 5f));
                break;
            case States.Patrol:
                StartCoroutine(ChangeStageButWaitNow(States.Patrol, States.Patrol, 5f));
                break;
            case States.WalkToAttack:
                ai.ChangeStage(shotPriority ? States.Shot : States.Attack);
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
