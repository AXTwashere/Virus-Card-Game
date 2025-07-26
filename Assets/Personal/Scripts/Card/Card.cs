using DG.Tweening;
using NaughtyAttributes;
using TMPro;
using UnityEngine;

public class Card : MonoBehaviour
{
    [SerializeField, Expandable] CardInfo cardInfo;

    [HorizontalLine]

    [SerializeField] TMP_Text nameText;
    [SerializeField] TMP_Text descriptionText;
    [SerializeField] TMP_Text costText;
    [SerializeField] TMP_Text attackPowerText;
    [SerializeField] TMP_Text healthText;
    [SerializeField] SpriteRenderer artworkRenderer;

    [HorizontalLine]

    [SerializeField, ReadOnly] int health;

    public Transform enemyCard;

    void Start()
    {
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

    [Button]
    void Attack()
    {
        transform.DOMove(enemyCard.position, 0.5f).SetEase(Ease.InBack);
    }
}
