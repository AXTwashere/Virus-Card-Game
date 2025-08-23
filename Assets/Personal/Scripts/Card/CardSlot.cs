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
        card = newCard;
        card.onDeath.AddListener(cardDie);
        card.removeable = false;
    }

    public bool CanAddCard(){
        return this.card == null;
    }
    public void cardRemove(Card card) {
        this.card.onDeath.RemoveListener(cardDie);
        this.card = null; 
    }

    void cardDie() {
        deck.RemoveCard(card);
        cardRemove(null);
    }


}
