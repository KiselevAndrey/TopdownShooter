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

    int _bulletInMagazine;
    bool _canShot;
    bool _reloading;
    bool _playerReload;

    #region Awake Start Update
    void Awake()
    {
    }

    private void Start()
    {
        _canShot = true;
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
            Vector3 tempCrosshairPos = crosshairPos.position;
            tempCrosshairPos.x += (Random.value - .5f) * gunSO.spreadRadius;
            tempCrosshairPos.y += (Random.value - .5f) * gunSO.spreadRadius;

            Vector2 tempDirection = tempCrosshairPos - gunPos.position; 

            gunPos.up = tempDirection;

            Bullet bullet = Instantiate(gunSO.bulletPrefab, gunPos.position, gunPos.rotation).GetComponent<Bullet>();
            bullet.damage = Random.Range(minDamage, maxDamage);
            bullet.speed = gunSO.bulletSpeed;
        }

        _bulletInMagazine -= gunSO.ammoPerShot;

        audioSource.PlayOneShot(gunSO.shotClip);
        StartCoroutine(GunShot());
    }

    IEnumerator GunShot()
    {
        _canShot = false;
        yield return new WaitForSeconds(gunSO.fireRate);
        _canShot = true;
    }

    public bool CanShoot() => _canShot;
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
        _bulletInMagazine = newCountBullet;
        _reloading = false;

        if (!_playerReload) audioSource.PlayOneShot(gunSO.endReloadClip);
    }

    public void EndReload()
    {
        _playerReload = false;
    }
    #endregion

    #region Pick Up Down
    public void Pick(bool up)
    {
        sprites.SetActive(!up);
        bodyCollider.enabled = !up;
    }
    #endregion

    #region OnTrigger
    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case TagsNames.Player:
                PlayerAttack pa = collision.GetComponent<PlayerAttack>();
                if(pa)
                    pa.canPickUp = true;
                break;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case TagsNames.Player:
                PlayerAttack pa = collision.GetComponent<PlayerAttack>();
                if (pa)
                    pa.canPickUp = false;
                break;
        }
    }
    #endregion
}
