using DG.Tweening;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.Events;


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

    public UnityEvent<int> onDeath;
    public int index;

    void Start() {
        if (setupAtStart) SetUp(cardInfo, false);
    }

    public void SetUp(CardInfo newInfo, bool flipped)
    {
        if (flipped) transform.rotation = Quaternion.Euler(transform.rotation.x,180,transform.rotation.z);
        cardInfo = newInfo;
        Init();
        InitUI();
    }

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
        print(cardInfo.cardName);
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
