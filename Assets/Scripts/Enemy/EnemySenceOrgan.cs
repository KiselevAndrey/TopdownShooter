using UnityEngine;

public class EnemySenceOrgan : MonoBehaviour
{
    [SerializeField] Enemy enemy;

    #region OnTrigger
    private void OnTriggerEnter2D(Collider2D collision)
    {
        enemy.TriggerTreat(collision);
    }
    #endregion
}
