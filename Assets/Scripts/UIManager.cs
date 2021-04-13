using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Описание")]
    [SerializeField] Image descriptionImage;
    [SerializeField] Text descriptionText;

    [Header("Параметры оружия")]
    [SerializeField] Image weaponImage;
    [SerializeField] Sprite emptyWeaponSprite;
    [SerializeField] GameObject parametrs;
    [SerializeField] Text bulletInMagazine;
    [SerializeField] Text maxBulletInMagazine;
    [SerializeField] Text maxBulletInPlayer;

    [Header("Кол-во патронов")]
    [SerializeField] GameObject shotgunAmmo;
    [SerializeField] Text shotgunAmmoText;
    [SerializeField] GameObject pistolAmmo;
    [SerializeField] Text pistolAmmoText;
    [SerializeField] GameObject revolverAmmo;
    [SerializeField] Text revolverAmmoText;
    [SerializeField] GameObject rifleAmmo;
    [SerializeField] Text rifleAmmoText;
    [SerializeField] GameObject rocketAmmo;
    [SerializeField] Text rocketAmmoText;
    [SerializeField] GameObject plasmaAmmo;
    [SerializeField] Text plasmaAmmoText;


    #region OnEnable OnDestroy
    private void OnEnable()
    {
        Gun.OnPlayerNearly += ShowGunDescriptionAction;
        Gun.PickUpAction += PickUp;
        Gun.DropAction += Drop;
        Gun.ChangeBulletInMagasineAction += ChangeBulletInMagasine;
        AmmoInventory.ChangeBulletCount += ChangeBulletCount;
    }

    private void OnDestroy()
    {
        Gun.OnPlayerNearly -= ShowGunDescriptionAction;
        Gun.PickUpAction -= PickUp;
        Gun.DropAction -= Drop;
        Gun.ChangeBulletInMagasineAction -= ChangeBulletInMagasine;
        AmmoInventory.ChangeBulletCount -= ChangeBulletCount;
    }
    #endregion

    #region Gun
    void ShowGunDescriptionAction(bool show, string description)
    {
        descriptionImage.gameObject.SetActive(show);
        descriptionText.text = description + " \"E\"";
    }

    void PickUp(Sprite weapon, int bullets)
    {
        parametrs.SetActive(true);

        weaponImage.sprite = weapon;
        bulletInMagazine.text = bullets.ToString();
        maxBulletInMagazine.text = bullets.ToString();
    }

    void Drop()
    {
        parametrs.SetActive(false);
        weaponImage.sprite = emptyWeaponSprite;
    }

    void ChangeBulletInMagasine(int bullet)
    {
        bulletInMagazine.text = bullet.ToString();
    }
    #endregion

    #region Ammo Count
    void ChangeBulletCount(AmmoType ammoType, int newCount)
    {
        switch (ammoType)
        {
            case AmmoType.Shotgun:
                shotgunAmmoText.text = newCount.ToString();
                shotgunAmmo.SetActive(newCount > 0);
                break;

            case AmmoType.Pistol:
                pistolAmmoText.text = newCount.ToString();
                pistolAmmo.SetActive(newCount > 0);
                break;

            case AmmoType.Revolver:
                revolverAmmoText.text = newCount.ToString();
                revolverAmmo.SetActive(newCount > 0);
                break;

            case AmmoType.Rifle:
                rifleAmmoText.text = newCount.ToString();
                rifleAmmo.SetActive(newCount > 0);
                break;

            case AmmoType.Rocket:
                rocketAmmoText.text = newCount.ToString();
                rocketAmmo.SetActive(newCount > 0);
                break;

            case AmmoType.Plazma:
                plasmaAmmoText.text = newCount.ToString();
                plasmaAmmo.SetActive(newCount > 0);
                break;
        }
    }
    #endregion
}
