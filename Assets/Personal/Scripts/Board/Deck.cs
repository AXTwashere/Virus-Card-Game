using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;
using System.Collections;
using NaughtyAttributes;

public class Deck : MonoBehaviour
{
    public CardSpawner cardSpawner;
    public Hand hand;
    public Research research;
    public RectTransform rect;
    public bool IsEnemyDeck;

    public void GameStart()
    {
        
    }
    [Button]
    public void DrawCard() {
        if (IsEnemyDeck) return;
        Card card = cardSpawner.CreateCardPlayer(rect.position);
        card.OriginalParent = hand.GetComponent<AddCard>();
        Flip(card, () => {hand.AddCard(card);});
    }

    public void DrawCard(RectTransform rect)
    {
        if (IsEnemyDeck) return;
        Card card = cardSpawner.CreateCardPlayer(rect.position);
        card.OriginalParent = hand.GetComponent<AddCard>();
        Flip(card, () => { hand.AddCard(card); });
    }

    public void RemoveCard(Card card) {
        if (IsEnemyDeck) {
            Flip(card, () => {Destroy(card.gameObject); });
            return;
        }
        card.rect.DOMove(rect.position, 0.1f).OnComplete(() => {
            Flip(card, () => { Destroy(card.gameObject); });
        });
    }

    public void DrawResearch()
    {
        if (IsEnemyDeck) return;
        Card card = cardSpawner.CreateCardResearch(rect.position);
        card.OriginalParent = research.GetComponent<AddCard>();
        card.canvasGroup.blocksRaycasts = false;
        Flip(card, () => { research.addCard.AddNewCardNoInvoke(card);});
    }


    public Camera cam;
    float degreesPerSecond = 0.5f;
    Vector3 localAxis = Vector3.up;

    public void Flip(Card card, Action OnFlipComplete)
    {
        RectTransform rect = card.rect;
        if (card.flipping) return;
        card.flipping = true;

        if (cam == null)
        {
            var canvas = GetComponentInParent<Canvas>();
            if (canvas && canvas.worldCamera) cam = canvas.worldCamera;
        }

        float dotPrev = Vector3.Dot((rect.position - cam.transform.position).normalized, rect.forward);
        bool toggled = false;

        rect.DOLocalRotate(rect.localEulerAngles + localAxis * 180f, degreesPerSecond, RotateMode.FastBeyond360)
            .SetEase(Ease.Linear)
            .OnUpdate(() =>
            {
                Vector3 toCard = (rect.position - cam.transform.position).normalized;
                float dotNow = Vector3.Dot(toCard, rect.forward);

                if (!toggled)
                {
                    if ((dotPrev > 0f && dotNow <= 0f) || (dotPrev < 0f && dotNow >= 0f) || Mathf.Approximately(dotNow, 0f))
                    {
                        if (card.cardFront.active) card.cardFront.SetActive(false); 
                        else card.cardFront.SetActive(true);
                        toggled = true;
                    }
                }

                dotPrev = dotNow;
            })
            .OnComplete(() =>
            {
                card.flipping = false;
                OnFlipComplete?.Invoke();
            });
    }

    
}
