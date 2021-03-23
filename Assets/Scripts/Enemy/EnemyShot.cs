using UnityEngine;

public class EnemyShot : MonoBehaviour
{
    [Header("Параметры атаки")]
    [SerializeField, Min(0)] float minRange;
    [SerializeField, Min(0)] float maxRange;
    [SerializeField, Range(0, 2)] float minFireRate;
    [SerializeField, Range(0, 2)] float maxFireRate;

    [Header("Плевок")]
    [SerializeField] Transform gobPos;
    [SerializeField] GameObject spittlePrefab;

    [Header("Дистанции")]
    public float minDistance;
    public float maxDistance;

    Enemy _enemy;

    float timeLastShot;
    float _shotRange;
    float _fireRate;

    #region Awake Start Update
    void Awake()
    {
        _enemy = GetComponent<Enemy>();
    }

    private void Start()
    {
        _shotRange = Random.Range(minRange, maxRange);
        _fireRate = Random.Range(minFireRate, maxFireRate);

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
        if (timeLastShot >= _fireRate)
        {
            if (_enemy.direction.magnitude <= _shotRange)
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

    public void Shot() => Instantiate(spittlePrefab, gobPos.transform.position, gobPos.transform.rotation);
    #endregion
}
