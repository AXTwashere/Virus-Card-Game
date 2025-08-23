using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System.Collections.Generic;
using DG.Tweening;

public class BattleManager : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Deck deck;
    public Hand hand;
    public Research research;
    public Board board;
    public WaveStorer waveStorer;
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
    void EnemyTurnEnd() {
        TurnStart();
    }

    //select stuff
    public Card curSelectCard; //change later
    EventSystem es = EventSystem.current;
    bool mouseUp = true;
    public RectTransform ZoomUI;
    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            if (curSelectCard != null) { }
            PointerEventData data = new PointerEventData(es) { position = Input.mousePosition };
            RectTransform ok = null;
            if (data != null) ok = GetUIUnderMouse(data);
            if (ok != null)
            {
                Zoom zoom = ok.GetComponentInParent<Zoom>();
                if (zoom == null)
                {
                    curSelectCard = ok.GetComponentInParent<Card>();
                }
            }
            if (curSelectCard != null) { 
                ZoomUI.position = curSelectCard.rect.position;
                ZoomUI.gameObject.SetActive(true); 
            }
            else { ZoomUI.gameObject.SetActive(false); }
        }
    }

    //drag stuff
    Card curCard;
    AddCard originalParent;
    Canvas canvas;

    void Start() {
        waveStorer.endTurn.AddListener(EnemyTurnEnd);
        TurnStart();
        canvas = transform.root.GetComponent<Canvas>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        //ZOOM
        curSelectCard = null;
        ZoomUI.gameObject.SetActive(false);

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
        Player player = dropTarget.GetComponent<Player>();
        
        if (curCard.removeable && player != null) {
            AddCard target = dropTarget.GetComponent<AddCard>();
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
        CardSlot curCardSlot = originalParent.GetComponent<CardSlot>();
        if (enemy != null &&  curCardSlot != null && !curCard.attacked)
        {
            //change later for more attack
            if (curCardSlot.index == enemy.index)
            {
                attackAnimation(curCard, enemy, originalParent);
                curCard.attacked = true;
                Reset();
                return;
            }
        }

        //if none go back to original parent
        GoToParent();
        Reset();
    }
    void attackAnimation(Card card, Enemy enemy, AddCard parent) {
        card.rect.DOMove(enemy.transform.position, .2f).SetEase(Ease.InBack).OnComplete(() =>
        {
             enemy.TakeDamage(card.cardInfo.damage);
             parent.AddNewCard(card);
        });
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
