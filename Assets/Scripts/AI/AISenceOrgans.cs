using System.Collections;
using UnityEngine;

enum SenceOrganType { Ear, Eye, Nose }

public class AISenceOrgans : MonoBehaviour
{
    [SerializeField] AI ai;

    [Header("Свойства")]
    [SerializeField] SenceOrganType type;
    [SerializeField] LayerMask rayCastLayers;
    [SerializeField] LayerMask findingLayers;
    [SerializeField, Range(0, 100)] float chanceToDiscover;
    [SerializeField, Min(0)] float range;

    [SerializeField, Range(0.01f, 2f)] float secondsForNextFinding;

    //[SerializeField] 
    bool onlyTurn;
    [SerializeField] bool trowRayCast;

    [Header("Побочные данные")]
    [SerializeField] bool drawGizmo;
    [SerializeField] Color gizmoColor = Color.green;

    int _i = 0;

    #region OnEnable OnDisable OnDestroy
    private void OnEnable()
    {
        // запуск корутины на поиск таргета
        StartCoroutine(TryFindTarget());
    }

    private void OnDisable()
    {
        // остановка корутины на поиск таргета
        StopAllCoroutines();
    }
    #endregion

    IEnumerator TryFindTarget()
    {
        // кидаем оверлап
        Collider2D[] targets = Physics2D.OverlapCircleAll(transform.position, range, findingLayers);

        for (int i = 0; i < targets.Length; i++)
        {
            if (Random.value * 100 > chanceToDiscover) yield return new WaitForSeconds(secondsForNextFinding);

            Collider2DTreat(targets[i]);
        }
        yield return new WaitForSeconds(secondsForNextFinding);
        StartCoroutine(TryFindTarget());
    }

    #region Collider2DTreat
    void Collider2DTreat(Collider2D collider)
    {
        // если только через бросок рейкаста
        if (trowRayCast)
        {
            // если не цель, то выходим
            if (!TrowRayCast(collider))
                return;
        }

        SetEnemyOrOnlyTurn(collider);
    }

    void SetEnemyOrOnlyTurn(Collider2D collision)
    {
        if (onlyTurn) Turn(collision);
        else ai.SetTarget(collision.transform, playSound: true, tellSomeone: true);
    }

    void Turn(Collider2D collision)
    {
        Vector2 dir = Vector2.Lerp(ai.transform.up, collision.transform.position - ai.transform.up, 1);
        ai.transform.up = dir;
    }

    bool TrowRayCast(Collider2D collision)
    {
        // Узнаем вектор и дистанцию до collision.transfotm 
        Vector2 direction = collision.transform.position - ai.transform.position;
        float distance = direction.magnitude;

        // кидаем райкаст в collision.transfotm со слоями rayCastLayers
        RaycastHit2D hit2D = Physics2D.Raycast(ai.transform.position, direction, distance, rayCastLayers);
        //print(hit2D.collider.gameObject.layer);   // получили обычный int

        // если лайер хита будет collision.gameObject.layer (новым битовым) и chanceToDiscover подходит, то true
        return collision.gameObject.layer == hit2D.collider.gameObject.layer;
    }
    #endregion

    private void OnDrawGizmosSelected()
    {
        if (!drawGizmo) return;

        Gizmos.color = gizmoColor;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
