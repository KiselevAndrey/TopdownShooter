using System;
using System.Collections;
using UnityEngine;
public class AmmoItem : MonoBehaviour
{
    [Header("Объекты")]
    [SerializeField] Animator anim;
    [SerializeField] SpriteRenderer spriteRenderer;

    [Header("Переменные")]
    [SerializeField] float lifeTimeNormal;
    [SerializeField] float lifeTimePrepareDie;

    [Header("Для проверки")]
    [SerializeField] AmmoSO ammoSO;
    [SerializeField] int count;

    public static Action<AmmoType, int> PickUpAmmo; // для UI отображение 

    private void OnEnable()
    {
        StartCoroutine(LifeTime(lifeTimeNormal, true));
    }

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
        anim.SetBool(AnimParam.PickUp, true);
    }
    #endregion

    #region LifeTime Corutine
    IEnumerator LifeTime(float lifeTime, bool isNormalTime)
    {
        yield return new WaitForSeconds(lifeTime);

        if (isNormalTime)
        {
            anim.SetTrigger(AnimParam.PrepareDie);
            StartCoroutine(LifeTime(lifeTimePrepareDie, false));
        }
        else
            anim.SetTrigger(AnimParam.Dead);
    }
    #endregion

    // запуск в анимации
    public void Despawn() => Lean.Pool.LeanPool.Despawn(gameObject);
}
