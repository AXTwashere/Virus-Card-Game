using System.Collections.Generic;
using DG.Tweening;
using NUnit.Framework.Constraints;
using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;




public class Board : MonoBehaviour
{
    public Board enemyBoard;
    public Deck deck;

    public CardSlot[] cardSlots = new CardSlot[5];
    

    private void Start()
    {
        
    }

    public void TurnStart() { 
        foreach(CardSlot slot in cardSlots){ if (slot.card !=null) slot.card.attacked = false; }

    }


    //tests
    public Card test;
    [Button]
    void Test() { AddCard(1, test); }

    public void AddCard(int pos, Card card) {
        /*
        if (cardList[pos] != null) return false;
        cardList[pos] = card;
        card.index = pos;
        card.onDeath.AddListener(RemoveCard);

        card.rect.DOMove(cardSlots[pos].transform.position, 0.5f).SetEase(Ease.InBack).OnComplete(() => {
            card.rect.SetParent(cardSlots[pos]);
        });
        return true;
        */
    }

    public void AddResearch(Card card) { 
        /*
        researchCards++;
        card.rect.DOMove(researchTransform.position, 0.5f).SetEase(Ease.InBack).OnComplete(() => { 
            card.rect.SetParent(researchTransform); 
            researchLayout.spacing = (researchTransform.rect.height - (researchCards * card.rect.rect.height)) / (researchCards - 1);     
        });
        */
    }



    public void Attack(int pos, int enemyPos) {
        /*
        cardList[pos].rect.DOMove(enemyBoard.cardList[enemyPos].rect.position, 0.5f).SetEase(Ease.InBack);
        enemyBoard.cardList[enemyPos].TakeDamage(cardList[pos].cardInfo.damage);
        */
    }

    private void RemoveCard(int pos) {
        /*
        cardList[pos].rect.DOMove(deck.transform.position, 0.5f).SetEase(Ease.InBack).OnComplete(() => {
            //call Deck Animation
        });
        */
    }

}
