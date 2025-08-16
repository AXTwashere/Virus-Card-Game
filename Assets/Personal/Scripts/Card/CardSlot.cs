using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CardSlot : MonoBehaviour
{
    public int index;
    Board board;
    AddCard addCard;
    public Deck deck;
    public Card card = null;

    void Start() { 
        board = transform.parent.GetComponent<Board>();
        addCard = GetComponent<AddCard>();
        addCard.cardAdded.AddListener(cardAdded);
        addCard.cardRemove.AddListener(cardRemove);
    }


    public void cardAdded(Card newCard) {
        card = newCard;
        card.onDeath.AddListener(cardDie);
        card.removeable = false;
    }

    public bool CanAddCard(){
        return this.card == null;
    }
    public void cardRemove(Card card) { this.card = null; }

    void cardDie() {    
        card.rect.DOMove(deck.rect.position, 0.1f).OnComplete(() => {
            deck.RemoveCard(card);
        });
        cardRemove(card);
    }


}
