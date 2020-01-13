using UnityEngine;
using System.Collections;

public class Hex : MonoBehaviour
{

    // Our coordinates in the map array
    public int x;
    public int y;
    public bool thisBaseCamp = false;

    public Hex[] GetNeighbours()
    {

        // So if we are at x, y -- the neighbour to our left is at x-1, y
        Debug.Log(x);
        Debug.Log(y);
        GameObject leftNeighbour = GameObject.Find( (x - 1) + ", " + y);

        // Right neighbour is also easy to find.
        GameObject right = GameObject.Find( (x + 1) + ", " + y);

        // The problem is that our neighbours to our upper-left and upper-right
        // might be at x-1 and x, OR they might be at x and x+1, depending
        // on whether we our on an even or odd row (i.e. if y % 2 is 0 or 1)

        return null;
    }

}

