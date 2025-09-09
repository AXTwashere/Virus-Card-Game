using UnityEngine;

[CreateAssetMenu(fileName = "Waves1", menuName = "Scriptable Objects/Waves1")]
public class Waves1 : ScriptableObject
{
    public Wave[] waves;
}
[System.Serializable]
public class Row
{
    public CardInfo[] cards;
}
[System.Serializable]
public class Wave
{
    public Row[] rows;
}
