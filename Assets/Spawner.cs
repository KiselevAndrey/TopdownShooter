using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject prefab;
    [SerializeField] int count;
    [SerializeField] float range;

    private void OnEnable()
    {
        StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        for (int i = 0; i < count; i++)
        {
            Vector2 temp = transform.position;
            temp.x += (Random.value - 0.5f) * range;
            temp.y += (Random.value - 0.5f) * range;

            Instantiate(prefab, temp, Quaternion.identity);
            yield return new WaitForSeconds(0.5f);
        }
    }
}
