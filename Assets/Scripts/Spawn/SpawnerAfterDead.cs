using System.Collections;
using UnityEngine;

public class SpawnerAfterDead : MonoBehaviour
{
    [SerializeField] protected GameObject spawnedObject;
    [SerializeField] protected LayerMask layerOfDeathObject;
    [SerializeField] protected float range;
    [SerializeField] protected bool randomRotation;
    [SerializeField] protected float secondsBeforeSpawn;
    [SerializeField, Range(0, 100)] protected float spawnChance;

    #region On Enadle Disable Destroy
    private void OnEnable()
    {
        Health.ImDeath += Spawn;
    }

    private void OnDisable()
    {
        Health.ImDeath -= Spawn;
    }

    private void OnDestroy()
    {
        Health.ImDeath -= Spawn;
    }
    #endregion

    protected void Spawn(LayerMask layer, Vector2 position)
    {
        if ((layer & layerOfDeathObject) == 0) return; // значит в layerOfDeathObject нет ничего от layer

        StartCoroutine(Spawning(position));
    }

    protected IEnumerator Spawning(Vector2 position)
    {
        yield return new WaitForSeconds(secondsBeforeSpawn);

        Vector2 spawnPos = HelperVector.NewPointFromRange(position, range);
        
        Vector3 spawnRotation = Vector3.zero;
        if (randomRotation)
            spawnRotation.z = Random.Range(0, 360);

        Lean.Pool.LeanPool.Spawn(spawnedObject, spawnPos, Quaternion.Euler(spawnRotation));
    }
}
