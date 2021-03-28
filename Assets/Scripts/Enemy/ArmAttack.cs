using System.Collections.Generic;
using UnityEngine;

public class ArmAttack : MonoBehaviour
{
    [SerializeField] Enemy _enemy;
    [SerializeField] SpriteRenderer spriteRenderer;

    List<Health> healths = new List<Health>();

    public void Attack()
    {
        for (int i = 0; i < healths.Count; i++)
            healths[i].Hit(_enemy.attack.damage);
    }

    public void EnableSprite(bool value) => spriteRenderer.enabled = value;

    #region OnTrigger
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Health health = collision.GetComponent<Health>();

        if (health)
            healths.Add(health);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        Health health = collision.GetComponent<Health>();

        if (health)
            healths.Remove(health);
    }
    #endregion
}
