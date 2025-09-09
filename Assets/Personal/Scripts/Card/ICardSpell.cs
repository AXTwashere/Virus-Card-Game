using UnityEngine;

public interface ICardSpell
{
    bool NeedTarget();

    void OnPlaySpell();

    void OnPlaySpell(Card targetCard);

    bool CanCastSpellOn(Card targetCard);
}