using UnityEngine;
using NaughtyAttributes;

public class Enemy : MonoBehaviour
{
    CardSlot cardSlot;
    public bool inBoss = false;
    public bool attackable => cardSlot.card != null || inBoss;
    public int index => cardSlot.index;

    void Start() { cardSlot = GetComponent<CardSlot>(); }

    public void TakeDamage(int dmg) { cardSlot.card.TakeDamage(dmg); }
    [Button]
    public void test() { TakeDamage(1); }
}
