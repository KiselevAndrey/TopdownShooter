using UnityEngine;

[CreateAssetMenu(fileName = "AmmoSO")]
public class AmmoSO : ScriptableObject
{
    public Sprite sprite;
    public AmmoType type;
    [Range(1, 20)] public float countModiffier;
}
