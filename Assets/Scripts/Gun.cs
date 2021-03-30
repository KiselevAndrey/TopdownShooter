using UnityEngine;

public class Gun : MonoBehaviour
{
    [Header("Параметры")]
    [SerializeField, Range(0, 2)] float fireRate;
    [SerializeField, Min(0)] float damage;

    [SerializeField] Transform gunPos;
    [SerializeField] GameObject bulletPrefab;

    [Header("Звуки")]
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip shotClip;

    float _timeLastShot;
    bool _canShot;

    #region Awake Start Update
    void Awake()
    {
    }

    private void Start()
    {
        _timeLastShot = fireRate;
        _canShot = true;
    }

    private void Update()
    {
        if (!_canShot)
        {
            _timeLastShot += Time.deltaTime;
            _canShot = _timeLastShot >= fireRate;
        }
    }
    #endregion

    #region Shot
    public void CheckShot()
    {
        if (_canShot)
        {
            Shot();
            _timeLastShot = 0;
        }
    }

    public void Shot()
    {
        Bullet bullet = Instantiate(bulletPrefab, gunPos.transform.position, gunPos.transform.rotation).GetComponent<Bullet>();
        bullet.damage = damage;
        audioSource.PlayOneShot(shotClip);
    }
    #endregion
}
