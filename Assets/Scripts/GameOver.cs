using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class GameOver : MonoBehaviour
{
    //변수의 수가 많아 각 패널로 분류하여 가시성 높임
    [Header("Player Pannel")]
    public GameObject[] Player_pannel = new GameObject[4];
    [Header("Castle Number Pannel")]
    public TextMeshProUGUI[] castle_txt = new TextMeshProUGUI[4];
    [Header("Scoret Number Pannel")]
    public TextMeshProUGUI[] score_txt = new TextMeshProUGUI[4];

    public GameObject Win_Img;

    void Start()
    {
        //플레이어 2명일때 34번 패널 지움
        // for문보다 간편해보여 if문으로 2명일때 3명일때 작성 4명일때는 모두 오픈이므로 x

        if (NetworkRoundManager.player_Number == 2)
        {
            Player_pannel[2].SetActive(false);
            Player_pannel[3].SetActive(false);
        }
        if (NetworkRoundManager.player_Number == 3)
            Player_pannel[3].SetActive(false);
        //for (int i = 0; i < NetworkRoundManager.player_Number; i++)
        //    player_id[i].text = PhotonNetwork.PlayerList[i].NickName;//추후 닉네임변경
    }

    // Update is called once per frame
    void Update()
    {
        Player_Count();
    }
    //각 플레이어 패널에 카드수, 점령땅의 수, 현재 점수를 표현
    void Player_Count()
    {
        int temp = 0;
        int temp2 = 0;
        for (int i = 0; i < NetworkRoundManager.player_Number; i++)
        {
            castle_txt[i].text = EventManager.player_count[i].ToString();
            //차후 점수 표시
            //score_txt[i].text = NetworkRoundManager.                   .ToString();
            /*
             * if(temp > NetworkRoundManager.                   )
             * {
             *      return;
             * }else if(temp == NetworkRoundManger.            )
             * {
             *      int temp2 == temp;
             * }else
             * {
             *      temp = NetworkRoundManager.~~~~~~~~~~~;
             * }
             * 
             * 
             * */
        }
        // temp2가 0이면 temp1 이기게 처리
        // temp2가 != 0이면 temp1, 2 무승부처리
        // 그리고 이긴사람 img 띄우기
        //Win_Img.GetComponent<Image>().sprite = Resources.Load<Sprite>("이긴플레이어 id");
    }

    public void onClickExit()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.JoinLobby();
        PhotonNetwork.LoadLevel(0);
    }

}
