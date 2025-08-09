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

    public Card CreateCardPlayer(Vector2 position) {
        int index = Random.Range(0, playerCardInfos.Length);
        Card card = Instantiate(basicCardPrefab, board);
        card.rect.position = position;
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
        Card card = Instantiate(basicCardPrefab);
        card.SetUp(enemyCardInfos[index], true);

        //slide card into view
        //flip animation
        //on complete return card
        return card;
    }

    

}
