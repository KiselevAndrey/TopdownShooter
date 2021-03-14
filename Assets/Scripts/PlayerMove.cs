using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField, Range(0,10)] float speed;

    Rigidbody2D _rb;

    #region Awake Start Update
    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        Move();
    }

    void Update()
    {
        Rotate();        
    }
    #endregion

    #region Move & Rotate
    void Move()
    {
        float inputX = Input.GetAxis(AxesNames.Horizontal);
        float inputY = Input.GetAxis(AxesNames.Vertical);

        _rb.velocity = new Vector2(inputX, inputY).normalized * speed;
    }

    void Rotate()
    {
        Vector2 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mouseWorldPosition - (Vector2)transform.position;

        transform.up = direction;
    }
    #endregion
}
