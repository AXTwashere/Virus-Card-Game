using UnityEngine;
using NaughtyAttributes;

public class GameEnd : MonoBehaviour
{
    public BattleManager battleManager;
    [Button]
    public void Reset()
    {
        battleManager.ResetGame();
        gameObject.SetActive(false);
    }
}
