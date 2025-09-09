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
    public Board enemyBoard;



    private void Start()
    {
        //checks for 5 slot children
        CardSlot[] tempSlots = new CardSlot[5];
        for (int i = 0; i < 5; i++) {
            tempSlots[i] = transform.GetChild(i).GetComponent<CardSlot>();
            if (tempSlots[i] == null) return;
        }
        cardSlots = tempSlots;
    }

    public void TurnStart() { 
        foreach(CardSlot slot in cardSlots){
            if (slot.card != null)
            {
                slot.card.attacked = false;
                slot.card.cardAbility?.OnTurnStartAbility(this);
            }
        }
    }

    public void ResetBoard()
    {
        for (int i = 0; i < cardSlots.Length; i++)
        {
            cardSlots[i].cardDie(cardSlots[i].card);
        }
    }

    public void TurnEnd() {
        foreach (CardSlot slot in cardSlots)
        {
            if (slot.card != null)
            {
                slot.card.cardAbility?.OnTurnEndAbility(this);
            }
        }
    }

}
