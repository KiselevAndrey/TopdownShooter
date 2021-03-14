using UnityEngine;

public class EnemyShot : MonoBehaviour
{
    [SerializeField] float fireRange;
    [SerializeField, Range(0, 2)] float fireRate;

    [SerializeField] Transform gunPos;
    [SerializeField] GameObject bulletPrefab;

    Animator _anim;
    Enemy _enemy;

    float timeLastShot;

    #region Awake Start Update
    void Awake()
    {
        _anim = GetComponent<Animator>();
        _enemy = GetComponent<Enemy>();
    }

    private void Start()
    {
        timeLastShot = 0;
    }

    void Update()
    {
        if (_enemy.walk.CantUpdate()) return;

        CheckShot();
    }
    #endregion

    #region Shot
    void CheckShot()
    {
        if (timeLastShot >= fireRate)
        {
            if (_enemy.walk.direction.magnitude <= fireRange)
            {
                _anim.SetTrigger(AnimParam.Shot);
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
