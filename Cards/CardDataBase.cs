using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class CardDataBase : MonoBehaviour
{
    public static List<Card> cardList = new List<Card>();
    
    private void Awake()
    {
        cardList.Add(new Card(0, "Bluffing", Resources.Load<Sprite>("nameImage/bluffing") ,Resources.Load <Sprite>("iconImage/Spel51")));
        cardList.Add(new Card(1, "Attack", Resources.Load<Sprite>("nameImage/attack_png"), Resources.Load<Sprite>("iconImage/Attack")));
        cardList.Add(new Card(2, "Block", Resources.Load<Sprite>("nameImage/sheild_png"), Resources.Load<Sprite>("iconImage/shield2")));
        cardList.Add(new Card(3, "Recruit", Resources.Load<Sprite>("nameImage/recruit"), Resources.Load<Sprite>("iconImage/money")));
    }
}
