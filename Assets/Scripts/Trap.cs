using UnityEngine;

enum TrapType { OpenDoor, Awake }

public class Trap : MonoBehaviour
{
    [SerializeField] TrapType type;
    [SerializeField] LayerMask layer;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer != Mathf.Log(layer, 2)) return;

        switch (type)
        {
            case TrapType.OpenDoor:
                
                break;

            case TrapType.Awake:
                foreach(Transform child in transform)
                {
                    child.gameObject.SetActive(true);
                    child.parent = transform.parent;
                }
                break;
        }

        Destroy(gameObject);
    }
}
