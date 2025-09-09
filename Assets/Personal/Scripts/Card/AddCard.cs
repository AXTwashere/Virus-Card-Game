using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using NaughtyAttributes;



public class AddCard : MonoBehaviour
{
    public Card test;
    [Button]
    void Test() { AddNewCard(test); }

    public RectTransform rect;
    public UnityEvent<Card> cardAdded;

    void Start() { rect = GetComponent<RectTransform>(); }

    public bool AddNewCard(Card card)
    {
        card.OriginalParent = this;
        card.rect.DOMove(rect.position, 0.1f).OnComplete(() => {
            card.rect.SetParent(rect);
            cardAdded?.Invoke(card);
        });
        return true;
    }
    public bool AddNewCard(Card card,float time)
    {
        card.OriginalParent = this;
        card.rect.DOMove(rect.position, time).OnComplete(() => {
            card.rect.SetParent(rect);
            cardAdded?.Invoke(card);
        });
        return true;
    }
    public bool AddNewCard(Card card, float time, Ease ease)
    {
        card.OriginalParent = this;
        card.rect.DOMove(rect.position, time).SetEase(ease).OnComplete(() => {
            card.rect.SetParent(rect);
            cardAdded?.Invoke(card);
        });
        return true;
    }

    public void AddNewCardNoInvoke(Card card)
    {
        card.OriginalParent = this;
        card.rect.DOMove(rect.position, 0.1f).OnComplete(() => {
            card.rect.SetParent(rect);
        });
    }

    public UnityEvent<Card> cardRemove;

    public bool RemoveCard(Card card)
    {
        cardRemove?.Invoke(card);
        return true;
    }
}
