using UnityEngine;
using NaughtyAttributes;
using DG.Tweening;
using UnityEngine.Events;

public class Enemy : MonoBehaviour
{
    public Card card;
    public int index => card.index;
    public bool attackable = false;

    void Start() {

      card = GetComponent<Card>();
        
    }

    public void TakeDamage(int dmg) { 
        card.TakeDamage(dmg);
    }

    [Button]
    public void test() { TakeDamage(1); }



}
