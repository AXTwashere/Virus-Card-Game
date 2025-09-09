using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CardSlot : MonoBehaviour
{
    public int index;
    AddCard addCard;
    public Deck deck;
    public Card card = null;
    public RectTransform rect;
    void Start() { 
        rect = GetComponent<RectTransform>();

        addCard = GetComponent<AddCard>();
        if (addCard != null)
        {
            addCard.cardAdded.AddListener(cardAdded);
            addCard.cardRemove.AddListener(cardRemove);
        }
    }


    public void cardAdded(Card newCard) {
        if (newCard == null) return;
        card = newCard;
        card.onDeath.AddListener(cardDie);
        card.removeable = false;
        card.index = index;
    }

    public bool CanAddCard(){
        return this.card == null;
    }
    public void cardRemove(Card card) {
        if (this.card == null) return;
        this.card.onDeath.RemoveListener(cardDie);
        this.card = null; 
    }

    public void cardDie(Card card) {
        if (card == null) return;
        deck.RemoveCard(card);
        cardRemove(null);
    }

    public void CardUiCheck() {
        if (card != null) return;

        if (card.attacked)
        {
            //grey out
        }
        else {
            // not grey out
        }
        
    }


}
