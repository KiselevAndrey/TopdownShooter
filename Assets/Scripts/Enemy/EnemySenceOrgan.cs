using UnityEngine;


public class EnemySenceOrgan : MonoBehaviour
{
    [SerializeField] Enemy enemy;

    [Header("Свойства")]
    //[SerializeField] SenceOrganType type;
    [SerializeField] LayerMask rayCastLayers;
    [SerializeField] LayerMask findingLayers;
    [SerializeField, Range(0, 100)] float chanceToDiscover;
    [SerializeField, Min(0)] float range; 

    [SerializeField] int updaterCount;

    //[SerializeField] 
    bool onlyTurn;
    [SerializeField] bool trowRayCast;

    int _i = 0;

    #region OnEnable OnDisable OnDestroy
    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        
    }

    void OnDestroy()
    {

    }

    private void Update()
    {
        if (_i > updaterCount)
        {
            // кидаем оверлап
            Collider2D[] targets = Physics2D.OverlapCircleAll(transform.position, range, findingLayers);

            for (int i = 0; i < targets.Length; i++)
            {
                if (Random.value * 100 > chanceToDiscover) return;

                CollisionTreat(targets[i]);
            }

            _i = 0;
        }
        else _i++;
    }
    #endregion

    #region OnTrigger
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

        CollisionTreat(collision);
    }

    #region CollisionTreat
    void CollisionTreat(Collider2D collision)
    {
        // если только через бросок рейкаста
        if (trowRayCast)
        {
            // если не цель, то выходим
            if (!TrowRayCast(collision))
                return;
        }
        
        TrySetEnemy(collision);        
    }

    void TrySetEnemy(Collider2D collision)
    {
        if (onlyTurn) Turn(collision);
        else enemy.SetTarget(collision.transform, true);
    }

    void Turn(Collider2D collision)
    {
        Vector2 dir = Vector2.Lerp(enemy.transform.up, collision.transform.position - enemy.transform.up, 1);
        enemy.transform.up = dir;
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
}
