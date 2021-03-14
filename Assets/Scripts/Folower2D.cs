using UnityEngine;

enum MoveType { Lerp, MoveTowards, Velocity, Current}

public class Folower2D : MonoBehaviour
{
    public Transform target;
    [SerializeField] MoveType moveType;
    [SerializeField, Tooltip("Используется если moveType == Velocity")] bool isForvard;

    public float speed;

    Rigidbody2D _targetRB;
    float _startZ;

    #region Start Update
    private void Start()
    {
        _startZ = transform.position.z;

        if (!target)
            target = FindObjectOfType<Player>().GetComponent<Transform>();

        if (moveType == MoveType.Velocity) 
            _targetRB = target.GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (!target)
            target = FindObjectOfType<Player>().GetComponent<Transform>();

        Vector3 temp = target.transform.position;
        temp.z = _startZ;

        switch (moveType)
        {
            case MoveType.Lerp:
                transform.position = Vector3.Lerp(transform.position, temp, speed);
                break;

            case MoveType.MoveTowards:
                transform.position = Vector3.MoveTowards(transform.position, temp, speed);
                break;

            case MoveType.Velocity:
                transform.position = Vector3.MoveTowards(transform.position, temp + (Vector3)_targetRB.velocity * speed * (isForvard ? 1 : -1), speed);
                break;

            case MoveType.Current:
                transform.position = temp;
                break;
        }
    }
    #endregion
}
