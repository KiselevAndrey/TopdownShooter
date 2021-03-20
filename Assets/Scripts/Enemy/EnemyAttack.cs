using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] float attackRange;
    [SerializeField, Range(0, 2)] float attackRate;

    [Header("Дистанции")]
    public float maxDistance;
    public float minDistance;

    Enemy _enemy;

    float timeLastShot;

    #region Awake Start Update
    void Awake()
    {
        _enemy = GetComponent<Enemy>();
    }

    private void Start()
    {
        timeLastShot = 0;
    }

    void Update()
    {
        if (!_enemy.walk.CanUpdate()) return;

        CheckAttack();
    }
    #endregion

    #region Shot
    void CheckAttack()
    {
        if (timeLastShot >= attackRate)
        {
            if (_enemy.direction.magnitude <= attackRange)
            {
                _enemy.anim.SetTrigger(AnimParam.Shot);
                timeLastShot = 0;
            }
        }
        else
        {
            timeLastShot += Time.deltaTime;
        }
    }
    #endregion
}
