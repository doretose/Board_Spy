using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class CardToHand : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject Hand;
    public GameObject It;

    private Vector3 CardScale;
    private Vector3 baseScale = new Vector3(2.5f, 3f, 1f);
    private Vector3 highlightsScale = new Vector3(4f, 5f, 1f);

    public bool selectThisCard = false;
    // Use this for initialization
    void Start()
    {
        CardScale = baseScale;
    }

    // Update is called once per frame
    void Update()
    {
        Hand = GameObject.Find("Hand");
        It.transform.SetParent(Hand.transform);
        It.transform.localScale = CardScale;
        It.transform.position = new Vector3(transform.position.x, transform.position.y, -40);
        It.transform.eulerAngles = new Vector3(25, 0, 0);
        if (selectThisCard) CardScale = highlightsScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //카드 하이라이트
        //Debug.Log("OnMuouseEnter");
        CardScale = highlightsScale;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!selectThisCard)
        {
            //카드 하이라이트 종료
            //Debug.Log("OnMouseExit");
            CardScale = baseScale;
        }
    }
}
