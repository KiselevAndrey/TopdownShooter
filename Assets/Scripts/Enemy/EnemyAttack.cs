using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [Header("Параметры атаки")]
    [SerializeField, Min(0)] float damage;
    [SerializeField, Range(0, 2)] float attackRate;

    [Header("Дистанции")]
    public float minDistance;
    public float maxDistance;

    [Header("Атака рукой")]
    [SerializeField] GameObject armTrigger;
    [SerializeField] ArmAttack arm;
    [SerializeField] Collider2D selfBody;

    [HideInInspector] public bool isAttaking;

    Enemy _enemy;

    float timeLastShot;

    #region Awake Start Update
    void Awake()
    {
        _enemy = GetComponent<Enemy>();
    }

    private void Start()
    {
        timeLastShot = attackRate;
        arm.Starting(selfBody, damage);
    }

    void Update()
    {
        if (!_enemy.walk.CanUpdate()) return;

        CheckAttack();
    }
    #endregion

    #region Attack
    void CheckAttack()
    {
        if (timeLastShot >= attackRate)
        {
            if (_enemy.direction.magnitude <= maxDistance)
                Attack();
        }
        else
        {
            timeLastShot += Time.deltaTime;
        }
    }

    void Attack()
    {
        _enemy.anim.SetTrigger(AnimParam.Attack);
        isAttaking = true;
        timeLastShot = 0;
    }

    public void EndAttack() => isAttaking = false;

    public void ArmTriggerEnable() => armTrigger.SetActive(true);
    public void ArmTriggerDisable() => armTrigger.SetActive(false);
    #endregion
}
