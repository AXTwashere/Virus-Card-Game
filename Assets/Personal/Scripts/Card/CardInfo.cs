using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu]
public class CardInfo : ScriptableObject
{
    public string cardName;
    public string description;
    [ShowAssetPreview] public Sprite artwork;
    public int cost;
    public int health;
    public int damage;
    public int attackPower;
}
