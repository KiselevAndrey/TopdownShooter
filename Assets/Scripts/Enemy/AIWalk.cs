using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AIWalk : MonoBehaviour
{
    [Header("Скорости")]
    [SerializeField, Range(0, 10)] float minSpeed;
    [SerializeField, Range(0, 50)] float maxSpeed;

    [Header("Дистанции")]
    public bool trackingInfinityly;
    public float maxTrackingDistance;

    [Header("Доп данные")]
    [SerializeField] bool shotPriority;


    public void Walk(Vector3 target, bool patrol = false)
    {

    }
}
