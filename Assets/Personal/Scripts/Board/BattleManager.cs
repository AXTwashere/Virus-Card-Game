using DG.Tweening;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Deck deck;
    public Hand hand;
    public Research research;
    public Board board;
    public WaveStorer waveStorer;
    public Button endTurn;

    public EnemyAttackManager enemyBoard;
    bool gameOver = false;


    int difficulty = 2;
    float drawCardDelay = 0.4f;

    public static BattleManager battleManager;

    public enum PlayerType { User, Opponent }

    void Awake() { battleManager = this; }

    void Start()
    {
        endTurn.onClick.AddListener(EndTurnClicked);
        enemyBoard.playerTakeDmg.AddListener(TakeDamage);
        // RoundStart(); iE Numerator or tutorial button
        canvas = transform.root.GetComponent<Canvas>();

        
    }

    void TurnStart() {
        if (gameOver) return;
        endTurn.interactable = true;
        deck.DrawCard();
        board.TurnStart();
        research.TurnStart();
    }

    float delayPlayerSwitch = .5f;
    PlayerType currentPlayer = PlayerType.User;
    bool playerTurn => currentPlayer == PlayerType.User;
    
    
    IEnumerator CommitMove(PlayerType playerType)
    {
        yield return new WaitForSeconds(delayPlayerSwitch);
        // Check for game end conditions
        if (waveStorer.IsRoundOver())
        {
            //change difficulty later
            difficulty++;

            hand.ResetHand();
            board.ResetBoard();

            //go to item place
            print("round over");
            yield break;
        }

        // Cannot commit move if it's not the current player's turn
        if (playerType == currentPlayer)
            SwitchPlayers();

    }

    void SwitchPlayers() {
        currentPlayer = (currentPlayer == PlayerType.User) ? PlayerType.Opponent : PlayerType.User;
        if (currentPlayer == PlayerType.User) { 
            enemyBoard.enemyBoard.TurnEnd();
            TurnStart();
            // end turn button grey out
        }
        if (currentPlayer == PlayerType.Opponent) {
            board.TurnEnd();
            StartCoroutine(OpponentTurn()); 
        }
    }

    IEnumerator drawCardWithDelay(float delay, int i) {
        i--;
        deck.DrawCard();
        yield return new WaitForSeconds(delay);
        if (i > 0) { StartCoroutine(drawCardWithDelay( delay,  i)); }
    }

    IEnumerator OpponentTurn() {
        waveStorer.turnStart();
        yield return new WaitForSeconds(.3f);
        enemyBoard.AllAttack(() => {
            StartCoroutine(CommitMove(currentPlayer));
        }); 
        //attack stuff
    }

    public void GameStart() {
        playerHp = 25;
        RefreshUIHp();
        difficulty = 2;
        deck.DrawResearch();
        RoundStart();
    }

    [Button]
    public void RoundStart() {
        //change round system later and difficulty
        waveStorer.GenerateRandomRound(difficulty);
        waveStorer.turnStart();

        StartCoroutine(drawCardWithDelay(drawCardDelay,4)); 
      
        currentPlayer = PlayerType.User;
        TurnStart();
    }

    void EndTurnClicked() {
        endTurn.interactable = false;
        if (playerTurn) StartCoroutine(CommitMove(currentPlayer));
    }

    //player health 
    int playerHp;
    public TMP_Text playerHpText;

    void TakeDamage(int dmg) {
        playerHp -= dmg;
        if (playerHp <= 0) { 
            playerHp = 0;
            DOTween.KillAll();
            gameEnd.gameObject.SetActive(true); 
        }
        RefreshUIHp();
    }
    void RefreshUIHp() {
        playerHpText.text = playerHp.ToString();
    }

    //end screen
    public GameEnd gameEnd;

    public void ResetGame() { StartCoroutine(ResetGame1()); }
    IEnumerator ResetGame1() {
        gameOver = true;
        waveStorer.ResetBoard();
        enemyBoard.enemyBoard.ResetBoard();
        board.ResetBoard();
        research.ResetResearch();
        hand.ResetHand();
        yield return new WaitForSeconds(2f);
        gameOver = false;
        GameStart();
    }

    //select stuff
    public Card curSelectCard; //change later
    EventSystem es = EventSystem.current;
    bool mouseUp = true;
    public RectTransform ZoomUI;
    public Zoom zoom;
    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            if (zoom.inZoom) return;

            PointerEventData data = new PointerEventData(es) { position = Input.mousePosition };
            RectTransform ok = null;

            if (data != null) ok = GetUIUnderMouse(data);

            if (ok != null) {
                Zoom zoomTemp = ok.GetComponent<Zoom>();
                if (curSelectCard != null && zoomTemp != null) {
                    zoom.zoom(); 
                    ZoomUI.gameObject.SetActive(false); 
                    return; }
                curSelectCard = ok.GetComponentInParent<Card>();
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
    Canvas canvas;


    public void OnBeginDrag(PointerEventData eventData)
    {
        //ZOOM
        curSelectCard = null;
        ZoomUI.gameObject.SetActive(false);
        //check turn
        if (!playerTurn) return;
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
            GoToParent();
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
                curCard.cardAbility?.OnPlayAbility(board);
                Reset(); return;}

            Research researchTarget = dropTarget.GetComponent<Research>();
            if (researchTarget != null && curCard.cardInfo.cardType == "research"){
                target.AddNewCard(curCard);
                Reset(); return;}

        }
        //check opponent
        Enemy enemy = dropTarget.GetComponentInParent<Enemy>();
        CardSlot curCardSlot = curCard.OriginalParent.GetComponent<CardSlot>();
        if (enemy != null &&  curCardSlot != null && !curCard.attacked && enemy.attackable)
        {
            //change later for more attack
            if (curCardSlot.index == enemy.index)
            {
                attackAnimation(curCard, enemy);
                curCard.attacked = true;
                curCard.cardAbility?.OnAttackAbility(board, enemy.card);
                Reset();
                return;
            }
        }

        //if none go back to original parent
        GoToParent();
        Reset();
    }
    void attackAnimation(Card card, Enemy enemy) {
        card.rect.DOMove(enemy.transform.position, .2f).SetEase(Ease.InBack).OnComplete(() =>
        {
             enemy.TakeDamage(card.cardInfo.damage);
             card.OriginalParent.AddNewCard(card);
        });
    }
    void GoToParent() { curCard.OriginalParent.AddNewCard(curCard);}
    void Reset() {
        curCard.canvasGroup.blocksRaycasts = true;
        curCard = null;
    }

    RectTransform GetUIUnderMouse(PointerEventData eventData)
    {
        // Get the UI element under the mouse pointer
        var results = new List<RaycastResult>();
        canvas.GetComponent<GraphicRaycaster>().Raycast(eventData, results);
        return results.Count > 0 ? results[0].gameObject.GetComponent<RectTransform>() : null;
    }


}
