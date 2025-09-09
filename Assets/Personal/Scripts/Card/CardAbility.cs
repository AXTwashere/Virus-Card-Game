using UnityEngine;

public abstract class CardAbility : MonoBehaviour
{
    public abstract void Init(Card card);
    public abstract void OnPlayAbility(Board board);

    public abstract void OnDieAbility(Board board);

    public abstract void OnTurnStartAbility(Board board);

    public abstract void OnTurnEndAbility(Board board);

    public abstract void OnAttackAbility(Board board, Card cardAttacked);

    public abstract void OnKillAbility(Board board, Card cardKilled); //do later
}
