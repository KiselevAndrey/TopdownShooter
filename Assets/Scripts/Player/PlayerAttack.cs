using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Gun")]
    [SerializeField] Transform gunPos;
    [SerializeField] Gun gun;

    [Header("Arm")]
    [SerializeField] Transform armPos;
    [SerializeField] ArmAttack armAttack;
    [SerializeField, Range(0, 10)] float armDamage;
    [SerializeField, Range(0, 3)] float armRate;

    [HideInInspector] public bool canPickUp;

    Animator _anim;
    Player _player;
    Gun posibleWeapon;

    bool _canArmAttack = true;

    #region Awake Start Update
    void Awake()
    {
        _anim = GetComponent<Animator>();
        _player = GetComponent<Player>();
    }

    private void Start()
    {
    }

    void Update()
    {
        if (_player.IsDead()) return;

        CheckAttack();
        CheckPickUp();
    }
    #endregion

    #region Attack
    void CheckAttack()
    {
        if (gun)
            Shot();
        else if (Input.GetAxis(AxesNames.Fire1) > 0 && _canArmAttack)
        {
            StartCoroutine(AttackRate());
            _anim.SetTrigger(AnimParam.Attack);
        }
    }

    #region Shot
    void Shot()
    {
        if (Input.GetAxis(AxesNames.Fire1) > 0 && gun.CanShoot())
        {
            if (gun.TryShot())
                _anim.SetTrigger(AnimParam.Shot);
        }
        if (Input.GetKeyDown(KeyCode.R))
            gun.Reload();
        if (Input.GetKeyUp(KeyCode.R))
            gun.EndReload();
    }
    #endregion

    #region Arm
    void AttackArm()
    {
        armAttack.Attack(armDamage * Random.Range(0.9f, 1.1f));
    }

    IEnumerator AttackRate()
    {
        _canArmAttack = false;
        yield return new WaitForSeconds(armRate);
        _canArmAttack = true;
    }
    #endregion
    #endregion

    #region Pick Up Down
    void CheckPickUp()
    {
        // если нечего поднимать или нечего выбрасывать
        if (!canPickUp || !gun) return;

        if (Input.GetKeyUp(KeyCode.E))
        {
            if (gun) DropWeapon();
            if (canPickUp) PickUpWeapon();
        }
    }

    void DropWeapon()
    {
        gun.transform.parent = transform.parent;
        gun.Pick(false);
    }

    void PickUpWeapon()
    {
        gun = posibleWeapon;
        gun.transform.parent = gunPos;
        gun.Pick(true);
    }
    #endregion

    #region OnTrigger
    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case TagsNames.Weapon:
                posibleWeapon = collision.GetComponent<Gun>();
                break;
        }
    }
    #endregion
}
