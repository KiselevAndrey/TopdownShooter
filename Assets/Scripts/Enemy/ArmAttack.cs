using UnityEngine;

public class ArmAttack : MonoBehaviour
{
    Collider2D _body;
    float _damage;

    public void Starting(Collider2D body, float damage)
    {
        _body = body;
        _damage = damage;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Health health = collision.GetComponent<Health>();

        if (health)
            health.Hit(_damage);
    }
}
