using UnityEngine;

// набор скорости и торможение
// скорость разворота

public class EnemyWalk : MonoBehaviour
{
    [Header("Скорости")]
    [SerializeField, Range(0, 10)] float minSpeed;
    [SerializeField, Range(0, 10)] float maxSpeed;

    [Header("Дистанции")]
    public float maxTrackingDistance;

    [Header("Доп данные")]
    [SerializeField] bool shotPriority;

    Rigidbody2D _rb;
    Enemy _enemy;

    bool _walk;
    float _maxSpeed;
    float _minDistance;
    float _maxDistance;

    #region Awake Start Update
    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _enemy = GetComponent<Enemy>();
    }

    private void Start()
    {
        _maxSpeed = Random.Range(minSpeed, maxSpeed);

        _minDistance = shotPriority ? _enemy.shot.minDistance : _enemy.attack.minDistance;
        _maxDistance = shotPriority ? _enemy.shot.maxDistance : _enemy.attack.maxDistance;
    }

    private void FixedUpdate()
    {
        if (!CanUpdate()) return;

        Folloving();
    }
    #endregion

    public bool CanUpdate() => !_enemy.IsDead() && _enemy.target;

    #region Folloving
    void Folloving()
    {
        float distance = _enemy.direction.magnitude;

        _rb.velocity = distance > _minDistance ? _enemy.direction.normalized * _maxSpeed : Vector2.zero;

        if (distance < _minDistance)
            _rb.velocity = _enemy.direction.normalized * -_maxSpeed;

        if (distance > maxTrackingDistance)
        {
            _enemy.LoseTarget();
            _rb.velocity = Vector2.zero;
        }

        _enemy.anim.SetFloat(AnimParam.Speed, _rb.velocity.magnitude);

        // rotation
        transform.up = _enemy.direction;
    }

    void FollovingOld()
    {
        // move
        float distance = _enemy.direction.magnitude;

        if (_walk)
        {
            if (_enemy.attack.isAttaking) _rb.velocity = Vector2.zero;
            else _rb.velocity = _enemy.direction.normalized * _maxSpeed;

            _walk = distance >= _minDistance;
        }
        else
        {
            _rb.velocity = Vector2.zero;
            _walk = distance >= _maxDistance;
        }

        if(distance < _minDistance)
            _rb.velocity = _enemy.direction.normalized * -_maxSpeed;

        if (distance > maxTrackingDistance)
        {
            _enemy.LoseTarget();
            _rb.velocity = Vector2.zero;
        }

        _enemy.anim.SetFloat(AnimParam.Speed, _rb.velocity.magnitude);

        // rotation
        transform.up = _enemy.direction;
    }
    #endregion

    public void DontMove()
    {
        _rb.velocity = Vector2.zero;
        _rb.bodyType = RigidbodyType2D.Static;
    }
}
