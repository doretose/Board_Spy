using UnityEngine;
using System.Collections;

public class CardBack : MonoBehaviour
{
    public GameObject cardBack;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(GetComponent<ThisCard>().cardBack == true)
            
        {
            cardBack.SetActive(true);
        }
        else
        {
            cardBack.SetActive(false);
        }
    }
}
