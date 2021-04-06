using UnityEngine;

public enum AmmoType { Shotgun, Pistol, Revolver, Rifle, Rocket, Plazma }

[CreateAssetMenu(fileName = "GunSO")]
public class GunSO : ScriptableObject
{
    [Header("Параметры выстрела")]
    [Range(0, 2)] public float fireRate;
    [Range(0, 3), Tooltip("Радиус разброса")] public float spreadRadius;
    [Min(1), Tooltip("Сколько снарядов вылетает за выстрел")] public int bulletPerShot;
    [Min(1), Tooltip("Сколько патронов тратится за выстрел")] public int ammoPerShot;

    [Header("Параметры перезарядки")]
    [Min(1), Tooltip("Сколько патронов в обойме")] public int bulletInMagazine;
    [Tooltip("Заряжается по одному патрону")] public bool reloadOneBulletEach;
    [Min(0)] public float reloadTime;

    [Header("Параметры патронов")]
    public GameObject bulletPrefab;
    public AmmoType ammoType;
    public float bulletSpeed;

    [Header("Звуки")]
    public AudioClip shotClip;
    public AudioClip reloadAmmoClip;
    public AudioClip endReloadClip;
    public AudioClip noAmmoClip;

    [Header("Картинки")]
    public Sprite frontSprite;
}
