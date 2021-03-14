using UnityEngine;

public class Enemy : MonoBehaviour
{
    public EnemyShot shot;
    public EnemyWalk walk;

    #region Awake Start Update
    void Awake()
    {
    }

    private void Start()
    {
    }

    void Update()
    {
    }
    #endregion

    public bool IsDead() => false;
}
