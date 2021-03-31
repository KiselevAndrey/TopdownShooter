
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

    //[SerializeField] int updaterCount;
    //int _i;

    #region OnTrigger
    private void OnTriggerStay2D(Collider2D collision)
    {
        CheckCollider(collision);
    }

    #region TriggerTreat
    void CheckCollider(Collider2D collision)
    {
        // collision.gameObject.layer - обычный int
        // findingLayers - двоичное представление выбранных слоев

        // collision.gameObject.layer - переводим в двоичное представление с помощью 1 << collision.gameObject.layer (это на сколько битов влево двигаемся)
        int currentLayer = 1 << collision.gameObject.layer;

        // смотрим есть ли такой слой в findingLayers с помощью битового & (если есть, то ответ должен быть больше 0)
        if ((currentLayer & findingLayers) == 0) return;

        // Узнаем вектор и дистанцию до collision.transfotm 
        Vector2 direction = collision.transform.position - enemy.transform.position;
        float distance = direction.magnitude;

        // кидаем райкаст в collision.transfotm со слоями rayCastLayers
        RaycastHit2D hit2D = Physics2D.Raycast(enemy.transform.position, direction, distance, rayCastLayers);
        //print(hit2D.collider.gameObject.layer);   // получили обычный int

        // если лайер хита будет collision.gameObject.layer (новым битовым) и chanceToDiscover подходит, то срабатывает то что прописано для нужного органа чувств
        if (collision.gameObject.layer != hit2D.collider.gameObject.layer) return;
        if (Random.value * 100 > chanceToDiscover) return;
        print("сработало");
        switch (senceOrgan)
        {
            case SenceOrganType.Ear:
                break;
            case SenceOrganType.Eye:
                break;
            case SenceOrganType.Nose:
                break;
            default:
                break;
        }
        // енеми получает новый таргет и enemy.senceOrgans отключается
    }
    #endregion
    #endregion

    private void OnDestroy()
    {
        
    }
}
