using UnityEngine;

public class Player : MonoBehaviour
{
    PlayerAttack shot;
    PlayerMove move;
    Health health;

    [Header("Доп объекты")]
    public AudioSource audioSource;

    #region Awake Start Update
    void Awake()
    {
        health = GetComponent<Health>();
    }

    private void Start()
    {
    }

    void Update()
    {
    }
    #endregion

    public bool IsDead() => health.IsDead();

    #region OnCollision
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //switch (collision.gameObject.tag)
        //{
        //    case TagsNames.Bullet:
        //        Bullet bull = collision.gameObject.GetComponent<Bullet>();
        //        health.Hit(bull.damage);
        //        Destroy(bull.gameObject);
        //        break;
        //}
    }
    #endregion
}
