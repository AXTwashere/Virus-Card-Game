using UnityEngine;
using NaughtyAttributes;
using UnityEngine.Events;

public class WaveStorer : MonoBehaviour
{
    public CardSpawner cardSpawner;
    public BackSlot[] backSlots = new BackSlot[5];
    public UnityEvent endTurn;

    public int test = 0;
    [Button]
    public void tester() {
        Card card = cardSpawner.CreateCardEnemy();
        backSlots[test].AddCard(card);
    }
    public void turnStart() { }
    public void turnEnd() { endTurn.Invoke(); }
}
