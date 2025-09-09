using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class EnemyAttackManager : MonoBehaviour
{
    public Board enemyBoard;
    public Board playerBoard;
    public Canvas canvas;
    public bool gameEnd;

    public UnityEvent<int> playerTakeDmg;
    public void AllAttack(Action onComplete) {
        Attack(0, onComplete);
    }
    void Attack(int i, Action onComplete) {
        if (gameEnd) { return; }
        if (i > 4) { onComplete.Invoke(); return; }
        Card enemy = enemyBoard.cardSlots[i].card;

        //ADD LOGIC
        CardSlot target = playerBoard.cardSlots[i];
        bool NoAttack = false;
        if(enemy == null) NoAttack = true;
        else if (enemy.attacked) {NoAttack = true; enemy.attacked = false;}

        if (NoAttack) { StartCoroutine(AttackDone(i, onComplete)); return; }

        enemy.transform.SetParent(canvas.transform);
        enemy.rect.DOMove(target.rect.position, .2f).SetEase(Ease.InBack).OnComplete(() =>
        {

            StartCoroutine(AttackHit(i, target.card, enemy.damage));

            enemy.rect.DOMove(enemy.OriginalParent.transform.position, .1f).OnComplete(() =>
            {
                enemy.transform.SetParent(enemy.OriginalParent.transform);
            });

            StartCoroutine(AttackDone(i, onComplete));
        });
    }
    IEnumerator AttackHit(int i, Card target, int dmg) {
        if (target == null) { playerTakeDmg.Invoke(dmg); yield break; }
        target.TakeDamage(dmg);
        yield return new WaitForSeconds(.5f);      
    }

    IEnumerator AttackDone(int i, Action onComplete) {
        yield return new WaitForSeconds(.1f);
        Attack(i+1, onComplete);
    }
}
