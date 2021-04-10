using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Gun")]
    [SerializeField] Transform gunPos;
    [SerializeField] Gun gun;

    [Header("Arm")]
    [SerializeField] Transform armPos;
    [SerializeField, Range(0, 10)] float armDamage;
    [SerializeField, Range(0, 3)] float armRate;

    Animator _anim;
    Player _player;
    Gun _posibleWeapon;

    bool _canArmAttack = true;
    bool _canPickUp;

    #region Awake Start Update OnDestroy
    void Awake()
    {
        _anim = GetComponent<Animator>();
        _player = GetComponent<Player>();
        Gun.CanPickUp += CanPickUp;
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

    private void OnDestroy()
    {
        Gun.CanPickUp -= CanPickUp;
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
        if (Input.GetKey(KeyCode.R))
            gun.Reload();
        if (Input.GetKeyUp(KeyCode.R))
            gun.EndReload();
    }
    #endregion

    #region Arm
    void AttackArm()
    {
        //armAttack.Attack(armDamage * Random.Range(0.9f, 1.1f));
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
    void CanPickUp(bool value, Gun nearlyGun)
    {
        _canPickUp = value;
        _posibleWeapon = nearlyGun;
    }

    void CheckPickUp()
    {
        // если нечего поднимать и нечего выбрасывать
        if (!_canPickUp && !gun) return;

        if (Input.GetKeyUp(KeyCode.E))
        {
            if (gun && gun.CanShoot()) DropWeapon();
            if (_canPickUp) PickUpWeapon();
        }
    }

    void DropWeapon()
    {
        gunPos.gameObject.SetActive(false);
        gun.transform.parent = transform.parent;
        gun.Drop();
        gun = null;
    }

    void PickUpWeapon()
    {
        if (gun) return;
        gunPos.gameObject.SetActive(true);
        gun = _posibleWeapon;
        gun.transform.parent = gunPos;
        gun.PickUp();
    }
    #endregion
}
