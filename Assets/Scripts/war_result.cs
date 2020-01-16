using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class war_result : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //카드 하이라이트
        Debug.Log("OnMuouseEnter");
        //CardScale = highlightsScale;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("OnMouseExit");
        //if (!selectThisCard)
        //{
        //    //카드 하이라이트 종료
        //    Debug.Log("OnMouseExit");
        //    //CardScale = baseScale;
        //}
    }


}
