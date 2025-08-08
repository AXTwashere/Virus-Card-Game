using UnityEngine;
using DG.Tweening;

public class Deck : MonoBehaviour
{
    public CardSpawner cardSpawner;
    public Hand hand;

    public void GameStart()
    {
        
    }

    public void DrawCard() {
        Card card = cardSpawner.CreateCardPlayer();
        hand.AddCard(card);
    }

    public void RemoveCard(Card card) {
        cardSpawner.RemoveCard(card);
    }

    
    // Update is called once per frame
    void Update()
    {
        
    }
}
