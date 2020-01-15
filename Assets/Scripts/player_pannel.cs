using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

public class player_pannel : MonoBehaviour
{
    //변수의 수가 많아 각 패널로 분류하여 가시성 높임
    [Header("Player Pannel")]
    public GameObject[] Player_pannel = new GameObject[4];
    [Header("Card Text")]
    public TextMeshProUGUI[] card_text = new TextMeshProUGUI[4];
    [Header("Ground Text")]
    public TextMeshProUGUI[] ground_text = new TextMeshProUGUI[4];
    [Header("Score Text")]
    public TextMeshProUGUI[] score_text = new TextMeshProUGUI[4];
    [Header("Player Id")]
    public Text[] player_id = new Text[4];

    // public GameObject player_pannel_1, player_pannel_2, player_pannel_3, player_pannel_4;   

    //evnetmanager ==> 좌표로 차지한 땅수 표현
    //networkroundmanger ==> observer로 변경 후 가져가서 사용
    // 점수는 나중에!
    
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (NetworkRoundManager.nowRound == 0)
        {
            //플레이어의 설정한 닉네임을 패널에 표현
            for (int i = 0; i < NetworkRoundManager.player_Number; i++)
                player_id[i].text = PhotonNetwork.PlayerList[i].NickName;//추후 닉네임변경

            //플레이어 2명일때 34번 패널 지움
            // for문보다 간편해보여 if문으로 2명일때 3명일때 작성 4명일때는 모두 오픈이므로 x
            if (NetworkRoundManager.player_Number == 2)
            {
                Player_pannel[2].SetActive(false);
                Player_pannel[3].SetActive(false);
            }
            if (NetworkRoundManager.player_Number == 3)
                Player_pannel[3].SetActive(false);
        }
        else {
            switch (NetworkRoundManager.player_Number)
            {
                case 2:
                    {
                        Player_Count();
                        break;
                    }
                case 3:
                    {
                        Player_Count();
                        break;
                    }
                case 4:
                    {
                        Player_Count();
                        break;
                    }
            }
        }
    }
    //각 플레이어 패널에 카드수, 점령땅의 수, 현재 점수를 표현
    void Player_Count()
    {
        for (int i = 0; i < NetworkRoundManager.player_Number; i++)
        {
            ground_text[i].text = EventManager.player_count[i].ToString();
            card_text[i].text = NetworkRoundManager.cardNum[i].ToString();
            //차후 점수 표시
        }
    }
}
