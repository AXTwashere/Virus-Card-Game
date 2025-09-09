using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;
using NaughtyAttributes;

public class BackSlot : MonoBehaviour
{
    public CardSlot slot;
    RectTransform rect;
    AddCard addCard;

    int index;
    RectTransform spawnLoc;
    FrontSlots frontSlot;
    Deck deck;

    public bool hasCard => slot.card != null||cards.Count>0 || frontSlot.hasCard;

    List<Card> cards = new List<Card>();
    void Awake() {
        addCard = GetComponent<AddCard>();
        addCard.cardAdded.AddListener(CardAdded);

        frontSlot = GetComponentInParent<FrontSlots>();
        deck = GetComponentInParent<Deck>();

        rect = GetComponent<RectTransform>();
        slot = GetComponent<CardSlot>();

        spawnLoc = transform.GetChild(0).GetComponent<RectTransform>();
    }
    [Button]
    public void turnStart()
    {
        //move to front slot
        if (!frontSlot.hasCard) {
            
            Card card = slot.card;
            addCard.RemoveCard(card);
            frontSlot.AddCard(card);
            
            moveToBackSlot();
        }
    }
    

    
    public void moveToBackSlot() {
        if (slot.card == null && cards!=null && cards.Count>0)
        {
            Card card = cards[0];
            cards.Remove(card);
            
            card.canvasGroup.alpha = 1f;
            addCard.AddNewCard(card, .2f, DG.Tweening.Ease.InOutQuad);
            
        }
    }

    void CardAdded(Card card) {
        deck.Flip(card, () => { });
    }

    public void AddCard(Card card) {
        card.rect.position = spawnLoc.position;
        cards.Add(card);
        moveToBackSlot();
    }
}
