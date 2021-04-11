using System;
using UnityEngine;
public class AmmoItem : MonoBehaviour
{
    [Header("Объекты")]
    [SerializeField] Animator anim;
    [SerializeField] SpriteRenderer spriteRenderer;

    [Header("Переменные")]
    [SerializeField] float lifeTime;

    [Header("Для проверки")]
    [SerializeField] AmmoSO ammoSO;
    [SerializeField] int count;

    public static Action<AmmoType, int> PickUpAmmo; // для UI отображение 


    #region Create from Spawn
    public void SetAmmoSO(AmmoSO ammo)
    {
        ammoSO = ammo;
        spriteRenderer.sprite = ammo.sprite;
    }
    public void SetAmmo(int value) => count = (int)(value * ammoSO.countModiffier);
    #endregion

    #region From Player
    public AmmoType GetAmmoType() => ammoSO.type;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PickUpAmmo?.Invoke(GetAmmoType(), count);
        Lean.Pool.LeanPool.Despawn(gameObject);
    }
    #endregion
}
