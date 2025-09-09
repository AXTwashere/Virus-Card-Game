using NaughtyAttributes;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu]
public class CardInfo : ScriptableObject
{
    public bool isEnemy;
    public string cardName;
    public string cardName2;
    public string cardType; // object, spell, research, enemy
    public string description;
    [ShowAssetPreview] public Sprite artwork;
    public int cost;
    public int health;
    public int damage;
    public CardAbility cardAbility;
}
