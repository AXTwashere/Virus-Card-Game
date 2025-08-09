using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public Deck deck;
    public Hand hand;
    public Board board;

    public Card curCard;

    

    bool endGame;

    public static BattleManager battleManager;
    void Awake() { battleManager = this; }


}
