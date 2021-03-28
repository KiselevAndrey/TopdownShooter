using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [Header("Параметры атаки")]
    [SerializeField, Min(0)] float minDamage;
    [SerializeField, Min(0)] float maxDamage;
    [SerializeField, Range(0, 2)] float minAttackRate;
    [SerializeField, Range(0, 2)] float maxAttackRate;

    [Header("Дистанции")]
    public float minDistance;
    public float maxDistance;

    [Header("Атака рукой")]
    [SerializeField] ArmAttack arm;
    [SerializeField] Collider2D selfBody;

    [HideInInspector] public bool isAttaking;
    [HideInInspector] public float damage;

    Enemy _enemy;

    float _timeLastShot;
    float _attackRate;

    #region Awake Start Update
    void Awake()
    {
        _enemy = GetComponent<Enemy>();
    }

    private void Start()
    {
        _attackRate = Random.Range(minAttackRate, maxAttackRate);
        damage = Random.Range(minDamage, maxDamage);

        _timeLastShot = _attackRate;
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
        if (_timeLastShot >= _attackRate)
        {
            if (_enemy.direction.magnitude <= maxDistance)
                Attack();
        }
        else
        {
            _timeLastShot += Time.deltaTime;
        }
    }

    void Attack()
    {
        _enemy.anim.SetTrigger(AnimParam.Attack);
        isAttaking = true;
        _timeLastShot = 0;
    }

    public void EndAttack() => isAttaking = false;

    public void ArmTriggerEnable()
    {
        arm.EnableSprite(true);
        arm.Attack();
    }

    public void ArmTriggerDisable() => arm.EnableSprite(false);
    #endregion
}
