using UnityEngine;
using UnityEngine.EventSystems;

public class Hoverable : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public bool isHovering;

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovering = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovering = false;
    }
}
