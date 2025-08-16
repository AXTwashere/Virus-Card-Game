using DG.Tweening;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[RequireComponent(typeof(Button))]
public class Card : MonoBehaviour
{
    public bool setupAtStart;

    [SerializeField, Expandable] public CardInfo cardInfo;

    [HorizontalLine]

    [SerializeField] TMP_Text nameText;
    [SerializeField] TMP_Text descriptionText;
    [SerializeField] TMP_Text costText;
    [SerializeField] TMP_Text damageText;
    [SerializeField] TMP_Text healthText;
    [SerializeField] SpriteRenderer artworkRenderer;

    [HorizontalLine]

    [SerializeField, ReadOnly] int health;


    public RectTransform rect;
    public CanvasGroup canvasGroup;
    public GameObject cardFront;
    Button button;

    public UnityEvent onDeath;
    public int index;

    BattleManager battleManager;
    CardStack stack;
    Hoverable hoverable;


    public bool moveable;
    public bool removeable;
    public bool attacked = false;
    

    void Start() {
        button = GetComponent<Button>();
        button.onClick.AddListener(onButtonClick);
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
        battleManager = BattleManager.battleManager;

        moveable = true;

        removeable = true;

        health = cardInfo.health;

        hoverable.evtOnPointerEnter.AddListener(_ => { stack?.SetHovered(rect, true); });
        hoverable.evtOnPointerExit.AddListener(_ => { stack?.SetHovered(rect, false); });
    }

    void InitUI()
    {
        canvasGroup = GetComponent<CanvasGroup>();

        nameText.text = cardInfo.cardName;
        descriptionText.text = cardInfo.description;
        costText.text = cardInfo.cost.ToString();
        damageText.text = cardInfo.damage.ToString();
        healthText.text = cardInfo.health.ToString();

        if (cardInfo.artwork != null)
        {
            artworkRenderer.sprite = cardInfo.artwork;
        }
    }

    public void TakeDamage(int amount) { 
        health -= amount;
        if (health <= 0) {
            health = 0;
            onDeath?.Invoke();
        }
        RefreshUI();
    }

    void RefreshUI() {
        healthText.text = cardInfo.health.ToString();
    }

    public void SetStack(CardStack owner)
    {
        stack = owner;
    }

}
