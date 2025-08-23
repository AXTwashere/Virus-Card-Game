using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Zoom : MonoBehaviour
{
    
    public BattleManager battleManager;
    
    Card card;
    AddCard parent;
    Vector3 originalScale;
    bool originalMovable;
    bool originalRemovable;

    public Canvas canvas;
    RectTransform rect;
    bool inZoom = false;
    public GameObject Tint;
    public XButton xButton;

    void Start() {
        Tint.SetActive(false);
        rect = canvas.GetComponent<RectTransform>();
        xButton.Exit.AddListener(endZoom);
    }
 
    
    public void zoom() {
        card = battleManager.curSelectCard;
        battleManager.curSelectCard = null;
        if (card != null) {
            inZoom = true;

            Vector3 scale = card.transform.localScale;

            parent = card.GetComponentInParent<AddCard>();
            originalScale = scale;
            originalMovable = card.moveable;
            originalRemovable = card.removeable;

            Tint.SetActive(true);
            card.transform.SetParent(Tint.transform);

            card.moveable = false;
            card.removeable = false;
            //grow animation
            card.transform.localScale = new Vector3(scale.x * 2, scale.y * 2, 1);
            card.rect.anchoredPosition = new Vector2(0,0);
        }
        
    }
    
    void endZoom() {
        card.transform.SetParent(canvas.transform);
        Tint.SetActive(false);
        card.moveable = originalMovable;
        card.removeable = originalRemovable;
        //shrink animation
        card.transform.localScale = originalScale;
        parent.AddNewCard(card);
        inZoom = false;
    }
    
}
