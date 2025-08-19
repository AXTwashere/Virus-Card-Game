using System.Collections.Generic;
using DG.Tweening;
using NUnit.Framework.Constraints;
using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;




public class Board : MonoBehaviour
{
    public Deck deck;

    public CardSlot[] cardSlots = new CardSlot[5];
    public Enemy[] enemySlots = new Enemy[5];



    private void Start()
    {
    }

    public void TurnStart() { 
        foreach(CardSlot slot in cardSlots){ if (slot.card !=null) slot.card.attacked = false; }
    }

    


}
