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
    [SerializeField] TMP_Text attackPowerText;
    [SerializeField] TMP_Text healthText;
    [SerializeField] SpriteRenderer artworkRenderer;

    [HorizontalLine]

    [SerializeField, ReadOnly] int health;


    public RectTransform rect;
    public GameObject cardFront;
    Button button;

    public UnityEvent<int> onDeath;
    public int index;

    BattleManager battleManager;
    

    void Start() {
        button = GetComponent<Button>();
        button.onClick.AddListener(onButtonClick);
        battleManager = BattleManager.battleManager;
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
        if (cardInfo == null)
        {
            print("CardInfo is not assigned!");
            return;
        }

        health = cardInfo.health;
    }

    void InitUI()
    {
        if (cardInfo == null)
        {
            print("CardInfo is not assigned!");
            return;
        }
        nameText.text = cardInfo.cardName;
        descriptionText.text = cardInfo.description;
        costText.text = cardInfo.cost.ToString();
        attackPowerText.text = cardInfo.attackPower.ToString();
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
            onDeath?.Invoke(index);
        }
        RefreshUI();
    }

    void RefreshUI() {
        healthText.text = cardInfo.health.ToString();
    }

    

}
