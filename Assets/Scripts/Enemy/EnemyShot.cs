using UnityEngine;

public class EnemyShot : MonoBehaviour
{
    [SerializeField] float shotRange;
    [SerializeField, Range(0, 2)] float fireRate;

    [SerializeField] Transform gunPos;
    [SerializeField] GameObject bulletPrefab;

    [Header("Дистанции")]
    public float maxDistance;
    public float minDistance;

    Enemy _enemy;

    float timeLastShot;

    #region Awake Start Update
    void Awake()
    {
        _enemy = GetComponent<Enemy>();
    }

    private void Start()
    {
        timeLastShot = 0;
    }

    void Update()
    {
        if (!_enemy.walk.CanUpdate()) return;

        CheckShot();
    }
    #endregion

    #region Shot
    void CheckShot()
    {
        if (timeLastShot >= fireRate)
        {
            if (_enemy.direction.magnitude <= shotRange)
            {
                _enemy.anim.SetTrigger(AnimParam.Shot);
                timeLastShot = 0;
            }
        }
        else
        {
            timeLastShot += Time.deltaTime;
        }
    }

    public void Shot() => Instantiate(bulletPrefab, gunPos.transform.position, gunPos.transform.rotation);
    #endregion
}
