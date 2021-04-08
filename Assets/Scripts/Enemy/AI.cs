using System.Collections.Generic;
using UnityEngine;

public enum States { Guard, Patrol, Walk, Attack, Shot, Idle }
public class AI : MonoBehaviour
{
    [Header("Описание")]
    [SerializeField, TextArea(1, 4)] string descriptions;

    [Header("Скрипты")]
    public AIWalk walk;
    public AIShot shot;
    public AIAttack attack;
    [SerializeField] Health health;

    [Header("Осн переменные")]
    [SerializeField] States startState;

    [Header("Основные объекты")]
    public Animator anim;
    [SerializeField] GameObject senceOrgans;

    [Header("Звуки")]
    [SerializeField] AudioSource audioSource;
    [SerializeField] List<AudioClip> findTarget;

    [HideInInspector] public States currentState;
    [HideInInspector] public Vector2 direction;
    [HideInInspector] public Transform target;
    [HideInInspector] public float distance;
    [HideInInspector] public bool isDie;

    #region Start Update
    private void Start()
    {
        currentState = startState;
    }

    private void Update()
    {
        
    }
    #endregion

    #region States
    void UpdateState()
    {
        switch (currentState)
        {
            case States.Guard:
                break;
            case States.Patrol:
                break;
            case States.Walk:
                break;
            case States.Attack:
                break;
            case States.Shot:
                break;
            case States.Idle:
                break;
        }
    }
    #endregion
}
