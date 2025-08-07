using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System.Collections.Generic;

public class Hand : MonoBehaviour
{
    public Board board;    
    Card curCard = null;
    RectTransform rect;

    public void addCard(Card card){
        card.rect.DOMove(rect.position, 0.5f).SetEase(Ease.InBack).OnComplete(() => {
            card.rect.SetParent(rect);
            //update spacing
        });
    }




}
