using UnityEngine;
using NaughtyAttributes;

public class WaveStorer : MonoBehaviour
{
    public CardSpawner cardSpawner;
    public BackSlot[] backSlots = new BackSlot[5];

    public int test = 0;
    [Button]
    public void tester() {
        Card card = cardSpawner.CreateCardEnemy();
        backSlots[test].AddCard(card);
    }
}
