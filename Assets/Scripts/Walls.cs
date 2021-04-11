using UnityEngine;

public class Walls : MonoBehaviour
{
    [SerializeField] int armor;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(TagsNames.Bullet))
        {
            if (collision.TryGetComponent(out Bullet bullet))
                bullet.HitSelf(armor);
        }
    }
}
