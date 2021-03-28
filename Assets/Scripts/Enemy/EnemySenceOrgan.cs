using UnityEngine;

public class EnemySenceOrgan : MonoBehaviour
{
    [SerializeField] Enemy enemy;

    //[SerializeField] int updaterCount;
    //int _i;

    #region OnTrigger
    private void OnTriggerEnter2D(Collider2D collision)
    {
        enemy.TriggerTreat(collision);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        enemy.TriggerTreat(collision);
    }
    #endregion
}
