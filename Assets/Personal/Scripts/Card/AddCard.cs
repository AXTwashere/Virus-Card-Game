using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using NaughtyAttributes;


// PLAYER ONLY

public class AddCard : MonoBehaviour
{
    public Card test;
    [Button]
    void Test() { AddNewCard(test); }

    RectTransform rect;
    public UnityEvent<Card> cardAdded;

    void Start() { rect = GetComponent<RectTransform>(); }

    public bool AddNewCard(Card card)
    {
        card.rect.DOMove(rect.position, 0.1f).OnComplete(() => {
            card.rect.SetParent(rect);
            cardAdded?.Invoke(card);
        });
        return true;
    }

    public UnityEvent<Card> cardRemove;

    public bool RemoveCard(Card card)
    {
        cardRemove?.Invoke(card);
        return true;
    }
}
