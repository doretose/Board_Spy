using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Card
{
    // 0 = None, 1 = 점령, 2 = 방어, 3 = 회유
    public int id;
    public string cardName;
    public Sprite cardNameImage;
    public Sprite thisImage;

    public Card()
    {

    }

    public Card(int id, string cardName, Sprite cardNameImage, Sprite thisImage)
    {
        this.id = id;
        this.cardName = cardName;
        this.cardNameImage = cardNameImage;
        this.thisImage = thisImage;
    }
}
