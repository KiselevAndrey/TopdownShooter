using UnityEngine;

public class Transition : MonoBehaviour
{
    [SerializeField] GameObject newLocation;
    [SerializeField] GameObject oldLocation;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case TagsNames.Player:
                oldLocation.SetActive(false);
                newLocation.SetActive(true);
                break;
        }
    }
}
