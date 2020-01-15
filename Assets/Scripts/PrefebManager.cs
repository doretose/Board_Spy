using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefebManager : MonoBehaviour
{

    public static void CreatePrefeb(GameObject isPrefeb, int locX, int locY, Color isColor)
    {
        GameObject go = GameObject.Find(locX + ", " + locY);
        MeshRenderer mr = go.GetComponent<MeshRenderer>();
        float pos_x = go.transform.position.x;
        float pos_z = go.transform.position.z;
        
        Instantiate(isPrefeb, new Vector3(pos_x, 1, pos_z), isPrefeb.transform.transform.rotation, go.transform);
        mr.material.color = isColor;
    }

    public static void CreatePrefeb(GameObject isPrefeb, int locX, int locY)
    {
        GameObject go = GameObject.Find(locX + ", " + locY);
        MeshRenderer mr = go.GetComponent<MeshRenderer>();
        float pos_x = go.transform.position.x;
        float pos_z = go.transform.position.z;

        Instantiate(isPrefeb, new Vector3(pos_x, 1, pos_z), isPrefeb.transform.transform.rotation, go.transform);
    }

}
