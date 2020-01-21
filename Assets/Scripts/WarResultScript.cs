using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WarResultScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
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
