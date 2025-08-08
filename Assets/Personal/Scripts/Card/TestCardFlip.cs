using System.Collections;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

public class TestCardFlip : MonoBehaviour
{
    [SerializeField] RectTransform rect;
    [SerializeField] GameObject backSide;
    [SerializeField] float degreesPerSecond = 0.5f;
    [SerializeField] Vector3 localAxis = Vector3.up;
    [SerializeField] Camera cam;

   bool flipping;
    public bool IsFlipping => flipping;

    [Button]
    public void Flip()
    {
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
                        backSide.SetActive(!backSide.activeSelf);
                        toggled = true;
                    }
                }

                dotPrev = dotNow;
            })
            .OnComplete(() => flipping = false);
    }
}
