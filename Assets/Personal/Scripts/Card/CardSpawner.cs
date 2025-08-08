using UnityEngine;

public class CardSpawner : MonoBehaviour
{
    public CardInfo[] playerCardInfos;
    public CardInfo[] enemyCardInfos;
    public Card basicCardPrefab;

    public Card CreateCardPlayer() {
        int index = Random.Range(0, playerCardInfos.Length);
        Card card = Instantiate(basicCardPrefab);
        card.SetUp(playerCardInfos[index], true);

        //flip animation
        // on complete return card
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
