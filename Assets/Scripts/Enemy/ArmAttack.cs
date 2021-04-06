using System.Collections.Generic;
using UnityEngine;

public class ArmAttack : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteRenderer;

    List<Health> _healths = new List<Health>();

    public void Attack(float damage)
    {
        for (int i = 0; i < _healths.Count; i++)
            _healths[i].Hit(damage);
    }

    public void EnableSprite(bool value)
    {
        if(spriteRenderer)
            spriteRenderer.enabled = value;
    }

    #region OnTrigger
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Health health = collision.GetComponent<Health>();

        if (health)
            _healths.Add(health);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        Health health = collision.GetComponent<Health>();

        if (health)
            _healths.Remove(health);
    }
    #endregion
}
