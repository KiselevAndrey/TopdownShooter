using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] float attackRange;
    [SerializeField, Range(0, 2)] float attackRate;

    Animator _anim;
    Enemy _enemy;

    float timeLastShot;

    #region Awake Start Update
    void Awake()
    {
        _anim = GetComponent<Animator>();
        _enemy = GetComponent<Enemy>();
    }

    private void Start()
    {
        timeLastShot = 0;
    }

    void Update()
    {
        if (_enemy.walk.CantUpdate()) return;

        CheckAttack();
    }
    #endregion

    #region Shot
    void CheckAttack()
    {
        if (timeLastShot >= attackRate)
        {
            if (_enemy.walk.direction.magnitude <= attackRange)
            {
                _anim.SetTrigger(AnimParam.Shot);
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
