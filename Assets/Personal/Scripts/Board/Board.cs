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
    public VerticalLayoutGroup researchLayout;

    public Card[] cardList = new Card[5];
    public RectTransform[] cardSlots = new RectTransform[5];

    public int researchPoints = 0;
    public int researchCards = 1;
    
    RectTransform researchTransform;

    private void Start()
    {
        researchTransform = researchLayout.GetComponent<RectTransform>();
        
    }
    //tests
    public Card test;
    [Button]
    void Test() { AddCard(1, test); }

    public bool AddCard(int pos, Card card) {
        if (cardList[pos] != null) return false;
        cardList[pos] = card;
        card.index = pos;
        card.onDeath.AddListener(RemoveCard);

        card.rect.DOMove(cardSlots[pos].transform.position, 0.5f).SetEase(Ease.InBack).OnComplete(() => {
            card.rect.SetParent(cardSlots[pos]);
        });
        return true;

    }

    public void AddResearch(Card card) { 
        researchCards++;
        card.rect.DOMove(researchTransform.position, 0.5f).SetEase(Ease.InBack).OnComplete(() => { 
            card.rect.SetParent(researchTransform); 
            researchLayout.spacing = (researchTransform.rect.height - (researchCards * card.rect.rect.height)) / (researchCards - 1);     
        });
        
    }



    public void Attack(int pos, int enemyPos) {
        cardList[pos].rect.DOMove(enemyBoard.cardList[enemyPos].rect.position, 0.5f).SetEase(Ease.InBack);
        enemyBoard.cardList[enemyPos].TakeDamage(cardList[pos].cardInfo.damage);
    }

    private void RemoveCard(int pos) {
        cardList[pos].rect.DOMove(deck.transform.position, 0.5f).SetEase(Ease.InBack).OnComplete(() => {
            //call Deck Animation
        });
    }

}
