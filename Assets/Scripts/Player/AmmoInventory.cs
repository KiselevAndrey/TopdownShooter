using System.Collections.Generic;
using UnityEngine;

public class AmmoInventory : MonoBehaviour
{
    Dictionary<AmmoType, int> ammoDict;

    private void OnEnable()
    {
        AmmoItem.PickUpAmmo += AddAmmo;
    }

    void AddAmmo(AmmoType ammo, int count)
    {
        ammoDict[ammo] += count;
    }

    public int GetAmmoCount(AmmoType type, int need = 0)
    {
        if (need == 0)
            return ammoDict[type];
        else
        {
            int temp = Mathf.Min(ammoDict[type], need);
            ammoDict[type] -= temp;
            return temp;
        }
    }
}
