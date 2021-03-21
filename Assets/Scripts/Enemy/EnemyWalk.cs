using UnityEngine;

// набор скорости и торможение
// скорость разворота

public class EnemyWalk : MonoBehaviour
{
    [Header("Скорости")]
    [SerializeField, Range(0, 10)] float maxSpeed;

    [Header("Дистанции")]
    public float maxTrackingDistance;

    [Header("Доп данные")]
    [SerializeField] bool shotPriority;

    Rigidbody2D _rb;
    Enemy _enemy;

    float _speed;
    bool _walk;

    #region Awake Start Update
    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _enemy = GetComponent<Enemy>();
    }

    private void Start()
    {
        
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
        // Move
        float distance = _enemy.direction.magnitude;
        float minDistance = shotPriority ? _enemy.shot.minDistance : _enemy.attack.minDistance;
        float maxDistance = shotPriority ? _enemy.shot.maxDistance : _enemy.attack.maxDistance;

        if (_walk)
        {
            if (_enemy.attack.isAttaking) _rb.velocity = Vector2.zero;
            else _rb.velocity = _enemy.direction.normalized * maxSpeed;

            _walk = distance >= minDistance;
        }
        else
        {
            _rb.velocity = Vector2.zero;
            _walk = distance >= maxDistance;
        }

        if(distance < minDistance)
            _rb.velocity = _enemy.direction.normalized * -maxSpeed;

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
