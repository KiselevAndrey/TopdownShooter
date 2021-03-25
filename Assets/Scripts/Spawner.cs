using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] List<GameObject> prefabs;
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
            Vector2 instatiatePos = transform.position;
            instatiatePos.x += (Random.value - 0.5f) * range;
            instatiatePos.y += (Random.value - 0.5f) * range;

            int j = Random.Range(0, prefabs.Count);
            Instantiate(prefabs[j], instatiatePos, Quaternion.Euler(0, 0, Random.value * 360), transform);
            yield return new WaitForSeconds(0.5f);
        }
    }
}
