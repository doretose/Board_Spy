using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class test_map_choice : MonoBehaviour
{
    Button redbtn, bluebtn, yellowbtn, choicebtn;
    public static MeshRenderer mr;
    string color;
    public GameObject flag_1, flag_2, flag_3, flag_4;
    public InputField input_text;
    Text text;

    //빨강 1 플레이어
    //Vector3 red_Color = new Vector3(255 /255f, 0 , 0);
    //파랑 2 플레이어
    //Vector3 blue_Color = new Vector3(83 / 255f, 147 / 255f, 224 / 255f);
    //노랑 3 플레이어
    //Vector3 yellow_Color = new Vector3(248 / 255f, 215 / 255f, 0);
    //보라 4 플레이어
    //Vector3 yellow_Color = new Vector3(168 / 255f, 0 / 255f, 255 / 255f);

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

    public void choice_click(Text text)
    {
        //좌표 읽어오기
        Debug.Log("choice");
        text.text = input_text.text;
        Debug.Log(text.text);
        GameObject go = GameObject.Find(text.text);
        mr = go.GetComponent<MeshRenderer>();
        float pos_x = go.transform.position.x;
        float pos_z = go.transform.position.z;

        switch (color)
        {
            case "red":
                {
                    Instantiate(flag_1, new Vector3(pos_x, 1, pos_z),  flag_1.transform.rotation);
                    mr.material.color = new Color(255 / 255f, 0, 0);
                    break;
                }
            case "blue":
                {
                    Instantiate(flag_2, new Vector3(pos_x, 1, pos_z), flag_2.transform.rotation);
                    mr.material.color = new Color(83 / 255f, 147 / 255f, 224 / 255f);
                    break;
                }
            case "yellow":
                {
                    Instantiate(flag_3, new Vector3(pos_x, 1, pos_z), flag_3.transform.rotation);
                    mr.material.color = new Color(248 / 255f, 215 / 255f, 0);
                    //mr.material = Resources.Load("player_choice_yellow", typeof(Material)) as Material;
                    break;
                }
        }
    }

}
