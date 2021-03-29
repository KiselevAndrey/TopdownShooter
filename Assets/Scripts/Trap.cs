using UnityEngine;

enum TrapType { OpenDoor }

public class Trap : MonoBehaviour
{
    [SerializeField] TrapType type;
    [SerializeField] LayerMask layer;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        print(collision.gameObject.layer);
        print(layer.value);
        print((int)layer == collision.gameObject.layer);
        if (collision.gameObject.layer != Mathf.Log(layer, 2)) return;

        switch (type)
        {
            case TrapType.OpenDoor:
                Destroy(gameObject);
                break;
        }
    }
}
