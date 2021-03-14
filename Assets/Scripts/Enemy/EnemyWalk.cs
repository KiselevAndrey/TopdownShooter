using UnityEngine;

public class EnemyWalk : MonoBehaviour
{
    public Transform target;
    [SerializeField, Range(0, 10)] float speed;

    [HideInInspector] public Vector2 direction;

    Rigidbody2D _rb;
    Animator _anim;
    Enemy _enemy;

    #region Awake Start Update
    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        _enemy = GetComponent<Enemy>();
    }

    private void Start()
    {
        
    }

    private void FixedUpdate()
    {
        if (CantUpdate()) return;

        Folloving();
    }
    #endregion

    public bool CantUpdate() => _enemy.IsDead() || !target;

    #region Folloving
    void Folloving()
    {
        direction = target.position - transform.position;

        _rb.velocity = direction.normalized * speed;
        _anim.SetFloat(AnimParam.Speed, _rb.velocity.magnitude);

        transform.up = direction;
    }
    #endregion

    #region OnTrigger
    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case TagsNames.Player:
                target = FindObjectOfType<Player>().GetComponent<Transform>();
                break;
        }
    }
    #endregion
}
