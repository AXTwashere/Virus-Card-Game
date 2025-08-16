using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Zoom : MonoBehaviour
{
    
    public BattleManager battleManager;
    /*
    Card card;
    AddCard parent;
    Vector3 originalScale;
    public Canvas canvas;
    RectTransform rect;
    bool inZoom = false;

    void Start() { rect = canvas.GetComponent<RectTransform>(); }
    
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && inZoom) endZoom();
    }
    */
    public void zoom() {
        battleManager.zoom();
        /*
        if (battleManager.curSelectCard != null) {
            inZoom = true;

            card = battleManager.curSelectCard;
            parent = card.GetComponentInParent<AddCard>();

            Vector2 scale = card.transform.localScale;
            originalScale = scale;
            
            card.transform.SetParent(canvas.transform);
            card.moveable = false;
            card.removeable = false;
            //grow animation
            card.transform.localScale = new Vector3(scale.x * 2, scale.y * 2, 0);
            card.rect.anchoredPosition = new Vector2(rect.rect.width,-rect.rect.height)/2;
        }
        */
    }
    /*
    void endZoom() {
        card.moveable = true;
        card.removeable = true;
        //shrink animation
        card.transform.localScale = originalScale;
        parent.AddNewCard(card);
        inZoom = false;
    }
    */
}
