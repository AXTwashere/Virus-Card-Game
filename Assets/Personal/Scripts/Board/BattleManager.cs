using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using DG.Tweening;

public class BattleManager : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Deck deck;
    public Hand hand;
    public Research research;
    public Board board;
    public bool playerTurn;
    bool ableToInteract;

    bool endGame;

    public static BattleManager battleManager;
    void Awake() { battleManager = this; }

    void TurnStart() {
        playerTurn = true;
        ableToInteract = true;
        deck.DrawCard();
        board.TurnStart();
        research.TurnStart();
    }

    //select stuff
    public Card curSelectCard; //change later
    EventSystem es = EventSystem.current;
    bool mouseUp = true;
    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            if (inZoom) endZoom();
        }
        if (Input.GetMouseButtonUp(0)) {
            if (ableToInteract)
            {
                PointerEventData data = new PointerEventData(es) { position = Input.mousePosition };
                RectTransform ok = null;
                if (data != null) ok = GetUIUnderMouse(data);
                if (ok != null) curSelectCard = ok.GetComponentInParent<Card>();
                if (curSelectCard != null) {/*effect + zoom*/}
                else { }
            }

            
        }
    }
    //zoom
    bool inZoom = false;
    AddCard zoomParent;
    Vector3 zoomOriginalScale;
    RectTransform canvasRect;
    public void zoom()
    {
        if (curSelectCard != null && ableToInteract)
        {
            inZoom = true;
            ableToInteract = false;

            zoomParent = curSelectCard.GetComponentInParent<AddCard>();

            Vector3 scale = curSelectCard.transform.localScale;
            zoomOriginalScale = scale;

            curSelectCard.transform.SetParent(canvas.transform);
            curSelectCard.moveable = false;
            curSelectCard.removeable = false;
            //grow animation
            curSelectCard.transform.localScale = new Vector3(scale.x * 2, scale.y * 2, 1);
            curSelectCard.rect.anchoredPosition = new Vector2(0, 0);
            
        }
    }
    void endZoom()
    {
        curSelectCard.moveable = true;
        curSelectCard.removeable = true;
        //shrink animation
        curSelectCard.transform.localScale = zoomOriginalScale;
        zoomParent.AddNewCard(curSelectCard);
        curCard = null;
        zoomParent = null;
        inZoom = false;
        ableToInteract = true;
    }

    //drag stuff
    Card curCard;
    AddCard originalParent;
    Canvas canvas;

    void Start() {
        TurnStart();
        canvas = transform.root.GetComponent<Canvas>();
        canvasRect = canvas.GetComponent<RectTransform>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!ableToInteract) { return; }
        //check for UI
        RectTransform pickUpTarget = GetUIUnderMouse(eventData);
        if (pickUpTarget == null) return;
        //check for Card
        curCard = pickUpTarget.GetComponentInParent<Card>();
        if (curCard == null) return;
        //check if is ok
        if (!curCard.moveable) {
            curCard = null;
            return;
        }
        //put Card on top
        originalParent = curCard.GetComponentInParent<AddCard>();
        curCard.transform.SetParent(canvas.transform);
        //set up for drop
        curCard.canvasGroup.blocksRaycasts = false;
    }
    public void OnDrag(PointerEventData eventData)
    {   //moves Card
        if (curCard == null) return;
        curCard.rect.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (curCard == null) return;
        //finds UI
        RectTransform dropTarget = GetUIUnderMouse(eventData);
        //checkValid
        if (dropTarget == null) {
            originalParent.AddNewCard(curCard);
            Reset();
            return;
        }
        //check slots
        AddCard target = dropTarget.GetComponent<AddCard>();
        
        if (curCard.removeable && target != null) {

            CardSlot slotTarget = dropTarget.GetComponent<CardSlot>();
            if (slotTarget != null && curCard.cardInfo.cardType == "object" && research.points>=curCard.cardInfo.cost && slotTarget.CanAddCard()) {
                research.removePoints(curCard.cardInfo.cost);
                target.AddNewCard(curCard);
                Reset(); return;}

            Research researchTarget = dropTarget.GetComponent<Research>();
            if (researchTarget != null && curCard.cardInfo.cardType == "research"){
                target.AddNewCard(curCard);
                Reset(); return;}

        }
        //check opponent
        Enemy enemy = dropTarget.GetComponent<Enemy>();
        if (enemy != null && originalParent.GetComponent<CardSlot>() != null)
        {
            curCard.rect.DOMove(enemy.transform.position, .3f).SetEase(Ease.InBack).OnComplete(() =>
            {
                enemy.TakeDamage(curCard.cardInfo.damage);
                GoToParent();
                Reset();
                return;
            });
        }

        //if none go back to original parent
        GoToParent();
        Reset();
    }
    void GoToParent() { if (originalParent!=null) originalParent.AddNewCard(curCard);}
    void Reset() {
        curCard.canvasGroup.blocksRaycasts = true;
        curCard = null;
        originalParent = null;
    }

    RectTransform GetUIUnderMouse(PointerEventData eventData)
    {
        // Get the UI element under the mouse pointer
        var results = new List<RaycastResult>();
        canvas.GetComponent<GraphicRaycaster>().Raycast(eventData, results);
        return results.Count > 0 ? results[0].gameObject.GetComponent<RectTransform>() : null;
    }


}
