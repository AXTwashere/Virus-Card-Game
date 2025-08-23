using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;
using DG.Tweening;

public class CardSpawner : MonoBehaviour
{
    public CardInfo[] playerCardInfos;
    public CardInfo[] enemyCardInfos;
    public Card basicCardPrefab;
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
        card.canvasGroup.blocksRaycasts = false;
        return card;
    }

    

}
