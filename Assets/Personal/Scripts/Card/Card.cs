using DG.Tweening;
using NaughtyAttributes;
using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public bool setupAtStart;

    [SerializeField, Expandable] public CardInfo cardInfo;
    public CardAbility cardAbility;

    [HorizontalLine]

    [SerializeField] TMP_Text nameText;
    [SerializeField] TMP_Text nameText2;
    [SerializeField] TMP_Text descriptionText;
    [SerializeField] TMP_Text costText;
    [SerializeField] TMP_Text damageText;
    [SerializeField] TMP_Text healthText;
    [SerializeField] SpriteRenderer artworkRenderer;

    [HorizontalLine]

    [SerializeField, ReadOnly] public int health;
    [SerializeField, ReadOnly] public int damage;


    public RectTransform rect;
    public CanvasGroup canvasGroup;
    public GameObject cardFront;

    public UnityEvent<Card> onDeath;
    public int index;

    CardStack stack;
    Hoverable hoverable;

    public bool moveable = false;
    public bool removeable = false;

    public bool attacked = true;
    public AddCard OriginalParent;

    void Start() {
        if (setupAtStart) SetUp(cardInfo, false);
    }

    void onButtonClick() { }

    public void SetUp(CardInfo newInfo, bool flipped)
    {
        if (flipped)
        {
            FlipToBack();
            transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
        }
        cardInfo = newInfo;
        Init();
        InitUI();
    }

    public void FlipToFront() { cardFront.SetActive(true); }
    public void FlipToBack() { cardFront.SetActive(false); }

    void Init()
    {
        hoverable = GetComponent<Hoverable>();

        if (cardInfo.cardAbility != null)
        {
            Type type = cardInfo.cardAbility.GetType();
            Component script = gameObject.AddComponent(  type );
            cardAbility = script as CardAbility;
            cardAbility.Init(this);
        }


        //haste
        attacked = true;
        if (cardInfo.isEnemy)
        {
            moveable = false;
            removeable = false;
        }
        else {
            moveable = true;
            removeable = true;
        }

            health = cardInfo.health;
        damage = cardInfo.damage;

        hoverable.evtOnPointerEnter.AddListener(_ => { stack?.SetHovered(rect, true); });
        hoverable.evtOnPointerExit.AddListener(_ => { stack?.SetHovered(rect, false); });
    }

    void InitUI()
    {
        canvasGroup = GetComponent<CanvasGroup>();

        nameText.text = cardInfo.cardName;
        nameText2.text = cardInfo.cardName2;
        descriptionText.text = cardInfo.description;

        if (cardInfo.cardType == "object")
        {
            costText.text = cardInfo.cost.ToString();
            damageText.text = cardInfo.damage.ToString();
            healthText.text = cardInfo.health.ToString();
        }
        else if (cardInfo.cardType == "spell") {
            costText.text = cardInfo.cost.ToString();
            damageText.enabled = false;
            healthText.enabled = false;
        }
        else if (cardInfo.cardType == "research")
        {
            costText.enabled = false;
            damageText.enabled = false;
            healthText.enabled = false;
        }


        if (cardInfo.artwork != null)
        {
            artworkRenderer.sprite = cardInfo.artwork;
        }
    }

    public void TakeDamage(int amount) { 
        health -= amount;
        if (health <= 0) {
            health = 0;
            //timer coroutine whatever;
            cardAbility?.OnDieAbility(GetComponentInParent<Board>());
            onDeath?.Invoke(this);
        }
        RefreshUI();
    }

    public void HealDamage(int amount) {
        health += amount;
        if (health > cardInfo.health) { 
            health = cardInfo.health; 
        }
        RefreshUI();
    }

    void RefreshUI() {
        healthText.text = health.ToString();
    }

    public void SetStack(CardStack owner)
    {
        stack = owner;
    }

    //flip
    public bool flipping = false;
   
}
