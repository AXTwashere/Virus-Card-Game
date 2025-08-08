using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System.Collections.Generic;
using NaughtyAttributes;

public class Hand : MonoBehaviour
{

    public Board board;    
    Card curCard = null;
    public RectTransform rect;

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


}
