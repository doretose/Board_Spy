using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class spyCardDeck : MonoBehaviour
{
    public List<Card> deck = new List<Card>();
    public static List<Card> staticDeck = new List<Card>();
    public List<Card> ShffleContainer = new List<Card>();
    
    public int x = 0;
    public static int deckSize;

    public GameObject CardToHand;
    public GameObject CardToHandAttack;

    public GameObject Hand;

    // Use this for initialization
    void Start()
    {
        deckSize = 50;
        for(int i =0; i < deckSize; i++)
        {
            if(i < 25)
            {
                deck[i] = CardDataBase.cardList[x];
                continue;
            }else if(i < 40){
                x = 2;
                deck[i] = CardDataBase.cardList[x];
                continue;
            }
            else
            {
                x = 3;
                deck[i] = CardDataBase.cardList[x];
                continue;
            }
        }
        Shuffle();
        deck[49] = CardDataBase.cardList[0];
        deck[48] = CardDataBase.cardList[2];
        deck[47] = CardDataBase.cardList[3];
    }

    void Update()
    {
        staticDeck = deck;
    }

    public void Shuffle()
    {
        for(int i =0; i < deckSize; i++)
        {
            ShffleContainer[0] = deck[i];
            int randomindex = Random.Range(0, deckSize);
            deck[i] = deck[randomindex];
            deck[randomindex] = ShffleContainer[0];
        }
    }
}
