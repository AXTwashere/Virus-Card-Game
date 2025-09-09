using System;
using UnityEngine;

public class FrontSlots : MonoBehaviour
{
    CardSlot cardSlot;
    AddCard addCard;
    int index;

    public bool hasCard => cardSlot.card != null;
    private void Start()
    {
        addCard = GetComponent<AddCard>();
        cardSlot = GetComponent<CardSlot>();
        index = cardSlot.index;
        addCard.cardAdded.AddListener(cardAdded);
    }
    public void cardAdded(Card newCard)
    {
        Enemy tempEnemy = newCard.GetComponent<Enemy>();
        if (tempEnemy != null) tempEnemy.attackable = true;
    }
    public void AddCard(Card card)
    {
        if (card == null) return;
        addCard.AddNewCard(card, .2f, DG.Tweening.Ease.InOutQuad);
    }
}
