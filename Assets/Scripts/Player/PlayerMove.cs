using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField, Range(0,10)] float speed;

    Player _player;
    Rigidbody2D _rb;
    Animator _anim;

    #region Awake Start Update
    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        _player = GetComponent<Player>();
    }

    private void FixedUpdate()
    {
        if (_player.IsDead()) return;

        Move();
        Rotate();
    }
    #endregion

    #region Move & Rotate
    void Move()
    {
        float inputX = Input.GetAxis(AxesNames.Horizontal);
        float inputY = Input.GetAxis(AxesNames.Vertical);

        _rb.velocity = new Vector2(inputX, inputY).normalized * speed;
        _anim.SetFloat(AnimParam.Speed, _rb.velocity.magnitude);
    }

    void Rotate()
    {
        Vector2 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mouseWorldPosition - (Vector2)transform.position;

        transform.up = direction;
    }
    #endregion
}
