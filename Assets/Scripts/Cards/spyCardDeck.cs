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
        deckSize = 40;
        for(int i =0; i < deckSize; i++)
        {
            if(i < 20)
            {
                deck[i] = CardDataBase.cardList[x];
                continue;
            }else if(i < 30){
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
        //StartCoroutine(StartGame());
    }

    // Update is called once per frame
    void Update()
    {
        staticDeck = deck;
    }

    IEnumerator StartGame()
    {
        for (int i = 0; i <= 1; i++)
        {
            yield return new WaitForSeconds(0.5f);
            Instantiate(CardToHandAttack, transform.position, transform.rotation);
        }
       for(int i = 0; i <= 2; i++)
        {
            yield return new WaitForSeconds(0.5f);
            Instantiate(CardToHand, transform.position, transform.rotation);
        }
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
