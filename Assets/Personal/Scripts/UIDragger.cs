using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform), typeof(CanvasGroup))]
public class UIDragger : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public UnityEvent OnPickup;
    public UnityEvent<RectTransform, bool> OnDrop;
    public bool snapBackToOriginalPosition = true;

    public Func<RectTransform, bool> IsValidDropTarget;

    Canvas canvas;
    RectTransform rectTransform;
    CanvasGroup canvasGroup;
    Transform originalParent;
    Vector2 originalPosition;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();

        canvas = transform.root.GetComponent<Canvas>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // Handle the beginning of the drag
        originalParent = transform.parent;
        originalPosition = rectTransform.anchoredPosition;

        transform.SetParent(canvas.transform);

        canvasGroup.blocksRaycasts = false;
        OnPickup?.Invoke();
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Handle the dragging
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Handle the end of the drag
        RectTransform dropTarget = GetUIUnderMouse(eventData);
        bool isValidDrop = IsValidDropTarget?.Invoke(dropTarget) ?? false;

        if (isValidDrop == false)
        {
            transform.SetParent(originalParent);
            if (snapBackToOriginalPosition)
            {
                if (transform.parent.TryGetComponent(out LayoutGroup group))
                {
                    LayoutRebuilder.ForceRebuildLayoutImmediate(group.GetComponent<RectTransform>());
                }
                else
                {
                    rectTransform.anchoredPosition = originalPosition;
                }
            }
        }

        originalParent = transform.parent;
        originalPosition = rectTransform.anchoredPosition;
        canvasGroup.blocksRaycasts = true;
        OnDrop?.Invoke(dropTarget, dropTarget != null);
    }

    RectTransform GetUIUnderMouse(PointerEventData eventData)
    {
        // Get the UI element under the mouse pointer
        var results = new List<RaycastResult>();
        canvas.GetComponent<GraphicRaycaster>().Raycast(eventData, results);
        return results.Count > 0 ? results[0].gameObject.GetComponent<RectTransform>() : null;
    }
}
