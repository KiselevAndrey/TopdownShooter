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


    #region OnEnable OnDestroy
    private void OnEnable()
    {
        Gun.OnPlayerNearly += ShowGunDescriptionAction;
        Gun.PickUpAction += PickUp;
        Gun.DropAction += Drop;
        Gun.ChangeBulletInMagasineAction += ChangeBulletInMagasine;
    }

    private void OnDestroy()
    {
        Gun.OnPlayerNearly -= ShowGunDescriptionAction;
        Gun.PickUpAction -= PickUp;
        Gun.DropAction -= Drop;
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
}
