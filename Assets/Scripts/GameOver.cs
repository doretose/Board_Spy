using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class GameOver : MonoBehaviourPunCallbacks
{
    //변수의 수가 많아 각 패널로 분류
    [Header("Player Pannel")]
    public GameObject[] Player_pannel = new GameObject[4];
    [Header("Castle Number Pannel")]
    public TextMeshProUGUI[] castle_txt = new TextMeshProUGUI[4];
    [Header("Scoret Number Pannel")]
    public TextMeshProUGUI[] score_txt = new TextMeshProUGUI[4];

    public TextMeshProUGUI gameResultText;
    public Sprite[] player_img = new Sprite[4];

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
    
    void Update()
    {
        if(PhotonNetwork.PlayerList.Length > 1) Player_Count();
    }

    //각 플레이어 패널에 카드수, 점령땅의 수, 현재 점수를 표현
    void Player_Count()
    {
        int temp = -1, temp2 = -1;
        for (int i = 0; i < NetworkRoundManager.player_Number; i++)
        {
            castle_txt[i].text = EventManager.player_count[i].ToString();
            //차후 점수 표시
            score_txt[i].text = EventManager.player_score[i].ToString();
            if (temp > EventManager.player_score[i]) break;
            else if(temp == EventManager.player_score[i]) temp2 = temp;
            else temp = EventManager.player_score[i];
        }

        if(temp == EventManager.player_score[NetworkRoundManager.public_Player_Id -1])
        {
            gameResultText.text = "You Win!!";
        } else { gameResultText.text = "You Lose..."; }

        if (temp2 != -1)
        {
            if (temp == temp2)
            {
                if (temp == EventManager.player_score[NetworkRoundManager.public_Player_Id -1])
                {
                    gameResultText.text = "Draw...";
                }
            }
        }
    }
    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(0);
    }

    IEnumerator left_Room()
    {
        PhotonNetwork.LeaveRoom();
        while (PhotonNetwork.InRoom)
        {
            yield return null;
        }
        SceneManager.LoadScene(0);
    }

    public void LeaveRoom()
    {
        StartCoroutine(left_Room());
    }
}
