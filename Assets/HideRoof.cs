using UnityEngine;

public class HideRoof : MonoBehaviour
{
    [SerializeField] GameObject roof;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == TagsNames.Player)
        {
            roof.SetActive(false);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == TagsNames.Player)
        {
            roof.SetActive(true);
        }
    }
}
