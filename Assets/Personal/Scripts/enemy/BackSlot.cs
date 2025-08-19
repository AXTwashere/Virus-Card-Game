using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;
using NaughtyAttributes;

public class BackSlot : MonoBehaviour
{
    CardSlot slot;
    RectTransform rect;

    RectTransform spawnLoc;
    CardSlot nextSlot;
    Deck deck;

    List<Card> cards = new List<Card>();
    void Start() {
        deck = GetComponentInParent<Deck>();
        rect = GetComponent<RectTransform>();
        nextSlot = transform.parent.GetComponent<CardSlot>();
        slot = GetComponent<CardSlot>();
        spawnLoc = transform.GetChild(0).GetComponent<RectTransform>();
    }
    [Button]
    public void turnStart()
    {
        //move to front slot
        if (nextSlot.card == null) {
            moveToFrontSlot(slot.card);
            nextSlot.cardAdded(slot.card);
            slot.cardRemove(null);
            moveToBackSlot();
        }
    }
    void moveToFrontSlot(Card card) {
        card.rect.DOMove(nextSlot.rect.position, .2f).OnComplete(() =>
        {
            card.rect.SetParent(nextSlot.transform);
        });
    }

    public void moveToBackSlot() {
        if (slot.card == null && cards!=null && cards.Count>0)
        {
            Card card = cards[0];
            slot.cardAdded(card);
            cards.Remove(card);
            card.canvasGroup.alpha = 1f;
            card.rect.DOMove(rect.position, .2f).OnComplete(() => {
                deck.Flip(card, () => {
                    card.rect.SetParent(rect);
                });
            });
        }
    }

    public void AddCard(Card card) {
        card.rect.position = spawnLoc.position;
        cards.Add(card);
        moveToBackSlot();
    }
}
