using UnityEngine;
using UnityEngine.UI;

public class Research : MonoBehaviour
{
    AddCard addCard;
    int numCards = 1;
    public int points = 0;

    public ResearchNumber researchNumber;

    VerticalLayoutGroup vertGroup;
    RectTransform rect;
    public float cardHeight;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        vertGroup = GetComponent<VerticalLayoutGroup>();
        rect = GetComponent<RectTransform>();
        addCard = GetComponent<AddCard>();
        addCard.cardAdded.AddListener(CardAdded);
    }

    void CardAdded(Card card) {
        numCards++;
        card.canvasGroup.blocksRaycasts = false;
        cardHeight = card.rect.rect.height;
        vertGroup.spacing = (rect.rect.height - (numCards * cardHeight)) / (numCards-1);
        card.moveable = false;
        card.removeable = false;
    }

    public void TurnStart() { 
        points += numCards;
        researchNumber.UpdateNumber(points);
    }

    public void removePoints(int num) {
        points -= num;
        researchNumber.UpdateNumber(points);
    }
}
