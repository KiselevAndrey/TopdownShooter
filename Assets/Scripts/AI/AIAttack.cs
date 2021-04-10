using System.Collections;
using UnityEngine;

public class AIAttack : MonoBehaviour
{
    [Header("Скрипты")]
    [SerializeField] AI ai;

    [Header("Осн переменные")]
    public float distanceForAttack;
    [SerializeField, Tooltip("x - min, y - max")] Vector2 damage;
    [SerializeField, Tooltip("x - min, y - max")] Vector2 rate;

    [Header("For Overlap")]
    [SerializeField] Transform attackPosition;
    [SerializeField] float range;
    [SerializeField] LayerMask layer;

    bool _canAttack = true;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPosition.position, range);
    }

    public void Enable(bool value)
    {
        this.enabled = value;
        attackPosition.gameObject.SetActive(value);
    }

    #region Attack
    public void Attack()
    {
        if (_canAttack)
        {
            ai.anim.SetTrigger(AnimParam.Attack);
            StartCoroutine(AttackRate());
        }
    }

    IEnumerator AttackRate()
    {
        _canAttack = false;
        yield return new WaitForSeconds(Random.Range(rate.x, rate.y));
        _canAttack = true;
    }

    public bool CanAttack() => _canAttack && ai.distance < distanceForAttack * 1.5f;
    #endregion

    #region Запуск из анимации
    public void Attacking()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(attackPosition.position, range, layer);

        foreach (Collider2D col in colliders)
        {
            if (col.TryGetComponent<Health>(out Health health))
            {
                health.Hit(Random.Range(damage.x, damage.y));
                if (health.IsDead() && health.transform == ai.attackTarget)
                    ai.LoseTarget();
            }
        }
    }

    public void EndAttack()
    {
        ai.ChangeStage(States.Idle);
    }
    #endregion
 }
