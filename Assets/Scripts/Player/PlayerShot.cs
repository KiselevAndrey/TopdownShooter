using UnityEngine;

public class PlayerShot : MonoBehaviour
{
    [SerializeField, Range(0, 2)] float fireRate;

    [SerializeField] Transform gunPos;
    [SerializeField] GameObject bulletPrefab;

    Animator _anim;
    float timeLastShot;

    #region Awake Start Update
    void Awake()
    {
        _anim = GetComponent<Animator>();
    }

    private void Start()
    {
        timeLastShot = 0;
    }

    void Update()
    {
        CheckShot();
    }
    #endregion

    #region Shot
    void CheckShot()
    {
        if (timeLastShot >= fireRate)
        {
            if (Input.GetAxis(AxesNames.Fire1) > 0)
            {
                _anim.SetTrigger(AnimParam.Shot);
                Shot();
                timeLastShot = 0;
            }
        }
        else
        {
            timeLastShot += Time.deltaTime;
        }
    }

    public void Shot() => Instantiate(bulletPrefab, gunPos.transform.position, gunPos.transform.rotation);

    public void StopGame() => Time.timeScale = 0;
    #endregion
}
