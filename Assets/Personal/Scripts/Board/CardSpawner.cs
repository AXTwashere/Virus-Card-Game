using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;
using DG.Tweening;

public class CardSpawner : MonoBehaviour
{
    public CardInfo research;
    public CardInfo[] playerCardInfos;
    public CardInfo[] enemyCardInfos;
    public Card basicCardPrefab;
    public Card EnemyCardPrefab;
    public RectTransform board;

    public Vector2 test;

    [Button]
    public void Test() {
        CreateCardPlayer(test);
    }

    public Card CreateCardPlayer(Vector3 position) {
        int index = Random.Range(0, playerCardInfos.Length);
        Card card = Instantiate(basicCardPrefab, board);
        card.rect.localPosition = card.transform.parent.InverseTransformPoint(position);
        card.SetUp(playerCardInfos[index], true);
        return card;
    }
    public Card CreateCardResearch(Vector3 position)
    {
        Card card = Instantiate(basicCardPrefab, board);
        card.rect.localPosition = card.transform.parent.InverseTransformPoint(position);
        card.SetUp(research, true);
        return card;
    }

    public void RemoveCard(Card card) {
        // flip animation
        //on complete death animation
        //on complete remove
    }

    public Card CreateCardEnemy()
    {
        int index = Random.Range(0, enemyCardInfos.Length);
        Card card = Instantiate(basicCardPrefab, board);
        card.SetUp(enemyCardInfos[index], true);
        card.canvasGroup.alpha = 0f;
        return card;
    }
    public Card CreateCardEnemy(CardInfo cardInfo)
    {
        Card card = Instantiate(EnemyCardPrefab, board);
        card.SetUp(cardInfo, true);
        card.canvasGroup.alpha = 0f;
        return card;
    }



}
