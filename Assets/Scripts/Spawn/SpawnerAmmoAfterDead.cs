using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerAmmoAfterDead : SpawnerAfterDead
{
    [SerializeField] List<AmmoSO> ammoSOs;
    [SerializeField] Vector2 ammoDropCount;

    protected new IEnumerator Spawning(Vector2 position)
    {
        yield return new WaitForSeconds(secondsBeforeSpawn);

        Vector2 spawnPos = HelperVector.NewPointFromRange(position, range);

        Vector3 spawnRotation = Vector3.zero;
        if (randomRotation)
            spawnRotation.z = Random.Range(0, 360);

        if(Lean.Pool.LeanPool.Spawn(spawnedObject, spawnPos, Quaternion.Euler(spawnRotation)).TryGetComponent(out AmmoItem ammo))
        {
            if(ammoSOs.Count > 0)
            {
                ammo.SetAmmoSO(ammoSOs[Random.Range(0, ammoSOs.Count)]);
                ammo.SetAmmo((int)Random.Range(ammoDropCount.x, ammoDropCount.y));
            }
        }
    }
}
