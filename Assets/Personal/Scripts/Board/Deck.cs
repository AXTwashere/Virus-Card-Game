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
    public RectTransform rect;
    public void GameStart()
    {
        
    }
    [Button]
    public void DrawCard() {
        Card card = cardSpawner.CreateCardPlayer(rect.position);
        Flip(card, () => {hand.AddCard(card);});
    }

    public void RemoveCard(Card card) {
        Flip(card, () => { Destroy(card); });
    }


    bool flipping = false;
    public Camera cam;
    float degreesPerSecond = 0.5f;
    Vector3 localAxis = Vector3.up;

    public void Flip(Card card, Action OnFlipComplete)
    {
        RectTransform rect = card.rect;
        if (flipping) return;
        flipping = true;

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
                        card.cardFront.SetActive(true);
                        toggled = true;
                    }
                }

                dotPrev = dotNow;
            })
            .OnComplete(() =>
            {
                flipping = false;
                OnFlipComplete?.Invoke();
            });
    }

    
}
