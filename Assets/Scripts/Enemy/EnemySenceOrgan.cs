
using UnityEngine;

enum SenceOrganType { Ear, Eye, Nose}

public class EnemySenceOrgan : MonoBehaviour
{
    [SerializeField] Enemy enemy;

    [Header("Свойства")]
    [SerializeField] SenceOrganType senceOrgan;
    [SerializeField] LayerMask rayCastLayers;
    [SerializeField] LayerMask findingLayers;
    [SerializeField, Range(0, 100)] float chanceToDiscover;

    [SerializeField] int updaterCount;

    int _i = 0;

    #region OnTrigger
    private void OnTriggerStay2D(Collider2D collision)
    {
        CheckCollider(collision);
    }

    void CheckCollider(Collider2D collision)
    {
        if(_i < updaterCount)
        {
            _i++;
            return;
        }
        _i = 0;

        // collision.gameObject.layer - обычный int
        // findingLayers - двоичное представление выбранных слоев

        // collision.gameObject.layer - переводим в двоичное представление с помощью 1 << collision.gameObject.layer (это на сколько битов влево двигаемся)
        int currentLayer = 1 << collision.gameObject.layer;

        // смотрим есть ли такой слой в findingLayers с помощью битового & (если есть, то ответ должен быть больше 0)
        if ((currentLayer & findingLayers) == 0) return;

        if (Random.value * 100 > chanceToDiscover) return;

        switch (senceOrgan)
        {
            case SenceOrganType.Ear:
                TreatHearing(collision);
                break;
            case SenceOrganType.Eye:
                TreatSight(collision);
                break;
            case SenceOrganType.Nose:
                TreatNose(collision);
                break;
        }
    }

    #region TriggerTreat
    void TreatHearing(Collider2D collision)
    {
        if (Random.value * 100 > chanceToDiscover)
        {
            Vector2 dir = Vector2.Lerp(enemy.transform.up, collision.transform.position - enemy.transform.up, 1);
            enemy.transform.up = dir;
            return;
        }

        enemy.SetTarget(collision.transform);
    }

    void TreatSight(Collider2D collision)
    {
        if (!TrowRayCast(collision)) return;

        enemy.SetTarget(collision.transform);
    }

    void TreatNose(Collider2D collision)
    {
        if (!TrowRayCast(collision)) return;

        enemy.SetTarget(collision.transform);
    }

    bool TrowRayCast(Collider2D collision)
    {
        // Узнаем вектор и дистанцию до collision.transfotm 
        Vector2 direction = collision.transform.position - enemy.transform.position;
        float distance = direction.magnitude;

        // кидаем райкаст в collision.transfotm со слоями rayCastLayers
        RaycastHit2D hit2D = Physics2D.Raycast(enemy.transform.position, direction, distance, rayCastLayers);
        //print(hit2D.collider.gameObject.layer);   // получили обычный int

        // если лайер хита будет collision.gameObject.layer (новым битовым) и chanceToDiscover подходит, то true
        return collision.gameObject.layer == hit2D.collider.gameObject.layer;
    }
    #endregion
    #endregion

    private void OnDestroy()
    {
        
    }
}
