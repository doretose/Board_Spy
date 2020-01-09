using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Tiles
{
    public int playerId;
    public int cardId;

    public Tiles()
    {

    }

    public Tiles(int id, int cardId)
    {
        playerId = id;
        this.cardId = cardId;
    }
}
