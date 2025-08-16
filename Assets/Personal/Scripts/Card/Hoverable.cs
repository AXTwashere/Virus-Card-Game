using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class Hoverable : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public bool isHoverable;
    public UnityEvent<Hoverable> evtOnPointerEnter;
    public UnityEvent<Hoverable> evtOnPointerExit;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isHoverable) evtOnPointerEnter?.Invoke(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isHoverable) evtOnPointerExit?.Invoke(this);
    }
}
