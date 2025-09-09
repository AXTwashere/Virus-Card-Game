using UnityEngine;
using NaughtyAttributes;
using UnityEngine.Events;
using NUnit.Framework;
using System.Collections.Generic;
using System;
using System.Linq;

public class WaveStorer : MonoBehaviour
{
    public CardSpawner cardSpawner;
    public BackSlot[] backSlots = new BackSlot[5];
    public Waves1 waves;
    public List<Row> CurRound;

    public int test = 0;
    [Button]
    public void tester() {
        Card card = cardSpawner.CreateCardEnemy();
        backSlots[test].AddCard(card);
    }
    [Button]
    public void testerGenerate()
    {
        GenerateRandomRound(1);
    }
    [Button]
    public void testerStartRound()
    {
        turnStart();
    }

    public void GenerateRandomRound(int numWaves) {
        CurRound = new List<Row>();
        while (numWaves > 0) {
            Wave wave = waves.waves[UnityEngine.Random.Range(0, waves.waves.Length)];
            foreach (Row row in wave.rows) { CurRound.Add(row); }
            numWaves--;
        }
    }

    public void turnStart(Action OnComplete = null) {
        for (int i = 0; i < 5; i++)
        {
           backSlots[i].turnStart();
           if (CurRound.Count > 0) { 
              CardInfo curCard = CurRound[0].cards[i];
               if (curCard != null) backSlots[i].AddCard(cardSpawner.CreateCardEnemy(curCard));
           }
        }
        if (CurRound.Count > 0) CurRound.RemoveAt(0);

        if (CurRound.Count < 2) {
            Wave wave = waves.waves[UnityEngine.Random.Range(0, waves.waves.Length)];
            foreach (Row row in wave.rows) { CurRound.Add(row); }
        }


        if(OnComplete!=null) OnComplete.Invoke();

    }

    public bool IsRoundOver() {
        bool result = true;
        foreach (BackSlot slot in backSlots) {
            result = result && !slot.hasCard;
        }
        return result && CurRound!=null && CurRound.Count <= 0;
    }

    public void ResetBoard()
    {
        CurRound = null;
        for (int i = 0; i < backSlots.Count(); i++)
        {
            backSlots[i].slot.cardDie(backSlots[i].slot.card);
            //Card card = backSlots[i].slot.card;
            //if (card != null) card.onDeath.Invoke(card);
        }
    }
}


