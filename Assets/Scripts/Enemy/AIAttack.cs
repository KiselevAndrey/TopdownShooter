using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAttack : MonoBehaviour
{
    [Header("Скрипты")]
    [SerializeField] AI ai;

    [Header("Осн переменные")]
    public float distanceForAttack;
    [SerializeField] Transform attackPosition;
    [SerializeField] float range;
    [SerializeField] LayerMask layer;

    public void Attack()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(attackPosition.position, range, layer);
    }
}
