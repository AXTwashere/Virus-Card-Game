using DG.Tweening;
using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public class Hand : MonoBehaviour
{

    public Board board;    
    public RectTransform rect;

    AddCard addCard;

    void Start() {
        addCard = GetComponent<AddCard>();
        addCard.cardAdded.AddListener(cardAdded);
        addCard.cardRemove.AddListener(cardRemove);
    }

    void cardAdded(Card card) { }
    void cardRemove(Card card) { }

    public void AddCard(Card card){
        
        card.rect.DOMove(rect.position, 0.5f).SetEase(Ease.InBack).OnComplete(() => {
            card.rect.SetParent(rect);
            //update spacing
        });
        
    }

    //tests
    public Card test;
    [Button]
    void Test() { AddCard(test); }

    public void ResetHand() {
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }

}
