using UnityEngine;

[CreateAssetMenu]
public class Gene : ScriptableObject
{
    [SerializeField]
    string geneName;
    public float minValue;
    public float maxValue;
}