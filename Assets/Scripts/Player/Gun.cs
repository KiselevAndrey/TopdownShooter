using System;
using System.Collections;
using UnityEngine;


public class Gun : MonoBehaviour
{
    [Header("Описание")]
    [SerializeField] string description;

    [Header("Осн объекты")]
    [SerializeField] GunSO gunSO;
    [SerializeField] Transform gunPos;
    [SerializeField] Transform crosshairPos;

    [Header("Осн переменные")]
    [SerializeField, Min(0)] float minDamage;
    [SerializeField, Min(0)] float maxDamage;

    [Header("Звуки")]
    [SerializeField] AudioSource audioSource;

    [Header("Доп объекты")]
    [SerializeField] GameObject sprites;
    [SerializeField] CircleCollider2D bodyCollider;
    [SerializeField] Animator anim;

    public static Action<bool, string> OnPlayerNearly;      // для показа оповещения отправляется UI
    public static Action<bool, Gun> CanPickUp;              // отправляется игроку чтобы показать что может поднять
    public static Action<Sprite, int> PickUpAction;         // для UI показать картинку оружия и кол-во патронов
    public static Action DropAction;                        // для UI показать что оружие выброшено
    public static Action<int> ChangeBulletInMagasineAction; // для UI обновить колво патронов

    int _bulletInMagazine;
    bool _canShot;
    bool _reloading;
    bool _playerReload;

    #region Awake Start Update
    private void OnEnable()
    {
        _canShot = true;
        _bulletInMagazine = 0;
    }
    #endregion

    #region Shot
    public bool TryShot()
    {
        if (_canShot && _bulletInMagazine > 0)
        {
            Shot();
            return true;
        }
        else if (_bulletInMagazine < 1 && !_reloading)
            NoAmmoShot();
        return false;
    }

    void NoAmmoShot()
    {
        audioSource.PlayOneShot(gunSO.noAmmoClip);
        StartCoroutine(GunShot());
    }

    void Shot()
    {
        for (int i = 0; i < gunSO.bulletPerShot; i++)
        {
            Vector2 tempCrosshairPos = crosshairPos.position;
            tempCrosshairPos.x += (UnityEngine.Random.value - .5f) * gunSO.spreadRadius;
            tempCrosshairPos.y += (UnityEngine.Random.value - .5f) * gunSO.spreadRadius;

            Vector2 tempDirection = tempCrosshairPos - (Vector2)gunPos.position; 

            gunPos.up = tempDirection;

            Bullet bullet = Lean.Pool.LeanPool.Spawn(gunSO.bulletPrefab, gunPos.position, gunPos.rotation).GetComponent<Bullet>();
            bullet.damage = UnityEngine.Random.Range(minDamage, maxDamage);
            bullet.speed = gunSO.bulletSpeed;
            bullet.SetLifeTime(gunSO.bulletLifeTime * UnityEngine.Random.Range(.9f, 1.1f));
            bullet.life = gunSO.bulletLife;
        }

        ChangeBulletInMagasine(_bulletInMagazine - gunSO.ammoPerShot);

        audioSource.PlayOneShot(gunSO.shotClip);
        StartCoroutine(GunShot());
    }

    IEnumerator GunShot()
    {
        _canShot = false;
        yield return new WaitForSeconds(gunSO.fireRate);
        _canShot = true;
    }

    public bool CanShoot() => _canShot && !_reloading;
    #endregion

    #region Reload
    public void Reload()
    {
        if (!_canShot || _bulletInMagazine >= gunSO.bulletInMagazine) return;

        _playerReload = true;

        if (_reloading) return;

        audioSource.PlayOneShot(gunSO.reloadAmmoClip);
        switch (gunSO.reloadOneBulletEach)
        {
            case true:
                StartCoroutine(Reloading(_bulletInMagazine + 1));
                break;

            case false:
                StartCoroutine(Reloading(gunSO.bulletInMagazine));
                break;
        }        
    }

    IEnumerator Reloading(int newCountBullet)
    {
        _reloading = true;

        yield return new WaitForSeconds(gunSO.reloadTime);
        ChangeBulletInMagasine(newCountBullet);

        _reloading = false;

        if (!_playerReload || _bulletInMagazine == gunSO.bulletInMagazine) audioSource.PlayOneShot(gunSO.endReloadClip);
    }

    void ChangeBulletInMagasine(int newCountBullet)
    {
        _bulletInMagazine = newCountBullet;
        ChangeBulletInMagasineAction?.Invoke(_bulletInMagazine);
    }

    public void EndReload()
    {
        _playerReload = false;
    }
    #endregion

    #region Pick Up Down
    public void PickUp()
    {
        Pick(true);

        //перезарядка
        audioSource.PlayOneShot(gunSO.reloadAmmoClip);
        StartCoroutine(Reloading(gunSO.bulletInMagazine));

        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        if (!_playerReload) audioSource.PlayOneShot(gunSO.endReloadClip);

        PickUpAction?.Invoke(gunSO.frontSprite, _bulletInMagazine);
    }

    public void Drop()
    {
        Pick(false);

        DropAction?.Invoke();
    }

    void Pick(bool up)
    {
        anim.SetBool(AnimParam.PickUp, up);
    }

    // отправляет рассылку UI, если он подписан
    void PlayerNearly(bool playerNearly)
    {
        CanPickUp?.Invoke(playerNearly, this);
        OnPlayerNearly?.Invoke(playerNearly, description);
    }
    #endregion

    #region OnTrigger
    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case TagsNames.Player:
                PlayerNearly(true);
                break;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case TagsNames.Player:
                PlayerNearly(false);
                break;
        }
    }
    #endregion
}
