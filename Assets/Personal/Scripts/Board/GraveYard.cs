using System.Collections.Generic;
using UnityEngine;

public class GraveYard : MonoBehaviour
{
    public List<GameObject> cards = new List<GameObject>();

    void CardAdded(Card newCard) {
        GameObject card = newCard.gameObject;
        cards.Insert(0,card);
        if (cards.Count > 5) {
            cards.RemoveAt(cards.Count-1);
        }
    }
}
