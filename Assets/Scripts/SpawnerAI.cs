using System.Collections;
using UnityEngine;

public class SpawnerAI : Spawner
{
    [Header("Доп параметры для зомби")]
    [SerializeField] Transform target;

    private void OnEnable()
    {
        if (!target) target = FindObjectOfType<Player>().transform;

        StartCoroutine(Spawn());
    }

    protected new IEnumerator Spawn()
    {
        for (int i = 0; i < count; i++)
        {
            Vector2 instatiatePos = transform.position;
            instatiatePos.x += (Random.value - 0.5f) * range;
            instatiatePos.y += (Random.value - 0.5f) * range;

            int j = Random.Range(0, prefabs.Count);

            AI ai = Lean.Pool.LeanPool.Spawn(prefabs[j], instatiatePos, Quaternion.Euler(0, 0, Random.value * 360), transform).GetComponent<AI>();

            ai.SetTarget(target, trackingInfinityly: true);

            yield return new WaitForSeconds(rateSpawn);
        }
    }
}
