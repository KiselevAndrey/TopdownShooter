using System;
using System.Collections.Generic;
using UnityEngine;

public class AmmoInventory : MonoBehaviour
{
    Dictionary<AmmoType, int> ammoDict;

    public static Action<AmmoType, int> ChangeBulletCount;

    #region On Enable/Disable/Destroy
    private void OnEnable()
    {
        AmmoItem.PickUpAmmo += AddAmmo;
    }
    private void OnDisable()
    {
        AmmoItem.PickUpAmmo -= AddAmmo;
    }
    private void OnDestroy()
    {
        AmmoItem.PickUpAmmo -= AddAmmo;
    }
    #endregion

    void AddAmmo(AmmoType ammo, int count)
    {
        ammoDict[ammo] += count;
        ChangeBulletCount?.Invoke(ammo, ammoDict[ammo]);
    }

    public int GetAmmoCount(AmmoType ammo, int need = 0)
    {
        if (need == 0)
            return ammoDict[ammo];
        else
        {
            int temp = Mathf.Min(ammoDict[ammo], need);
            AddAmmo(ammo, -temp);
            return temp;
        }
    }
}
