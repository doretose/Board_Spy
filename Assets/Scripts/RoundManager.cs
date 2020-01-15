using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class RoundManager : MonoBehaviour
{
    public bool isYourTurn;
    public int yourTurn;
    public int yourOponentTurn;
    public TextMeshProUGUI turnText;

    public int round;
    public TextMeshProUGUI roundText;
    public Button selectButton;
    public int playerId = 1;

    //Nullable Type int형같은 기본변수에 null값을 넣기위해서 HasValue 속성 = 값이 있으면 true 없으면 false, 
    public static int?[,] selectCAndT = new int?[1, 3];

    void Start()
    {
        isYourTurn = true;
        yourTurn = 1;
        yourOponentTurn = 0;

        round = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (isYourTurn == true)
        {
            turnText.text = "Your Turn";

        }
        else
        {
            turnText.text = "Oponent Turn";
        }
        roundText.text = round + "round";

        //카드, 타일 선택이 모두 완료되면 셀렉트버튼 활성화
        if (selectCAndT[0, 0].HasValue && MouseScripts.choice_Map == true)
        {
            selectButton.interactable = true;
        }
        else
        {
            selectButton.interactable = false;
        }

    }

    public void SelectButton()
    {
        //selectCAndT[0, 1], [0, 2] ==> x,y좌표
        Debug.Log("선택된 맵 x , y좌표 = " + "(" + MouseScripts.choice_Map_x + "," + MouseScripts.choice_Map_y + ")");

        //selectTiles[x][y].Add( playerId, cardid);
        selectCAndT[0, 1] = MouseScripts.choice_Map_x;
        selectCAndT[0, 2] = MouseScripts.choice_Map_y;

        //playerId, cardId
        GameObject.Find("EventSystem").GetComponent<EventManager>().setSelectTiles(selectCAndT[0, 1].Value, selectCAndT[0, 2].Value, playerId, selectCAndT[0, 0].Value);
        EventManager.tileLocX.Add(selectCAndT[0, 1].Value);
        EventManager.tileLocY.Add(selectCAndT[0, 2].Value);

        MouseScripts.mr.material.color = new Color(32 / 255f, 84 / 255f, 30 / 255f);
        MouseScripts.choice_Map = false;
        MouseScripts.ps.Stop();
        MouseScripts.ps.Clear();
        
        GameObject destoy_Card = GameObject.FindGameObjectWithTag("UseCard");
        Destroy(destoy_Card);

        EndYourTurn();
        selectCAndT[0, 0] = null;
    }

    public void EndYourTurn()
    {
        isYourTurn = false;
        yourOponentTurn += 1;
    }

    public void EndYourOponentTurn()
    {
        isYourTurn = true;
        yourTurn += 1;
        round += 1;
    }
}





