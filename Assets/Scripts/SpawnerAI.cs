using System.Collections;
using UnityEngine;

public class SpawnerAI : Spawner
{    
    [Header("Доп параметры для AI")]
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

            if (Lean.Pool.LeanPool.Spawn(prefabs[j], instatiatePos, Quaternion.Euler(0, 0, Random.value * 360), transform).TryGetComponent(out AI ai))
            {
                ai.SetTarget(target, trackingInfinityly: true);
                ai.ChangeStage(States.WalkToAttack);
            }

            yield return new WaitForSeconds(rateSpawn);
        }
    }
}
