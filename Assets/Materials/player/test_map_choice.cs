using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class test_map_choice : MonoBehaviour
{
    Button redbtn;
    Button bluebtn;
    Button yellowbtn;
    Button choicebtn;

    string color;
    public InputField input_text;
    Text text;

    public void red_choice_color()
    {
        color = "red";
        Debug.Log("red");
    }

    public void yellow_choice_color()
    {
        color = "yellow";
        Debug.Log("yellow");
    }

    public void blue_choice_color()
    {
        color = "blue";
        Debug.Log("blue");
    }

    public void choice_click()
    {
        //좌표 읽어오기
        Debug.Log("choice");

        //내일 인풋필드에 좌표 입력받고
        //입력받은 좌표 ==> 변경눌렀을때
        // 그 좌표 색깔 변경되게 설정
        //==> 색변경은 asset/materials/player
        //폴더 안에 있는 3가지 materials 사용하여 변경

        //차후  플레이어 색 변경으로 사용

    }

}
