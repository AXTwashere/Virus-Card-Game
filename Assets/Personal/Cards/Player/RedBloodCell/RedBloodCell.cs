using UnityEngine;

public class RedBloodCell : CardAbility
{

    Card card;
    public override void Init(Card card) {
        this.card = card;
    }

    public override void OnPlayAbility(Board board) { }

    public override void OnDieAbility(Board board) { }

    public override  void OnTurnStartAbility(Board board) { }

    public override void OnTurnEndAbility(Board board) {
        int index = card.index;
        if(index+1<5 && board.cardSlots[index+1].card!=null) board.cardSlots[index+1].card.HealDamage(1);
        if (index - 1 >= 0 && board.cardSlots[index - 1].card != null) board.cardSlots[index - 1].card.HealDamage(1);
    }

    public override void OnAttackAbility(Board board, Card cardAttacked) { }

    public override void OnKillAbility(Board board, Card cardKilled) { }
}
