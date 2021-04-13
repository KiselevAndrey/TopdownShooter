using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] protected List<GameObject> prefabs;
    [SerializeField] protected int count;
    [SerializeField] protected float range;
    [SerializeField] protected float rateSpawn = 0.5f;

    [Header("Побочные данные")]
    [SerializeField] bool drawGizmo;
    [SerializeField] Color gizmoColor = Color.yellow;

    private void OnDrawGizmosSelected()
    {
        if (!drawGizmo) return;

        Gizmos.color = gizmoColor;
        Gizmos.DrawWireSphere(transform.position, range);
    }

    private void OnEnable()
    {
        StartCoroutine(Spawn());
    }

    protected IEnumerator Spawn()
    {
        for (int i = 0; i < count; i++)
        {
            Vector2 instatiatePos = transform.position;
            instatiatePos.x += (Random.value - 0.5f) * range;
            instatiatePos.y += (Random.value - 0.5f) * range;

            int j = Random.Range(0, prefabs.Count);
            Lean.Pool.LeanPool.Spawn(prefabs[j], instatiatePos, Quaternion.Euler(0, 0, Random.value * 360), transform);
            yield return new WaitForSeconds(rateSpawn);
        }
    }
}
