using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class player_pannel : MonoBehaviour
{
    public int Player_Id;
    int player_1_card, player_2_card, player_3, player_4;

    [Header("Player 1 Pannel")]
    public GameObject Player_pannel_1;
    public TextMeshProUGUI card_text_1;
    public TextMeshProUGUI ground_text_1;
    public TextMeshProUGUI score_text_1;
    public Text player_1_id;

   [Header("Player 2 Pannel")]
    public GameObject Player_pannel_2;
    public TextMeshProUGUI card_text_2;
    public TextMeshProUGUI ground_text_2;
    public TextMeshProUGUI score_text_2;
    public Text player_2_id;

    [Header("Player 3 Pannel")]
    public GameObject Player_pannel_3;
    public TextMeshProUGUI card_text_3;
    public TextMeshProUGUI ground_text_3;
    public TextMeshProUGUI score_text_3;
    public Text player_3_id;

    [Header("Player 4 Pannel")]
    public GameObject Player_pannel_4;
    public TextMeshProUGUI card_text_4;
    public TextMeshProUGUI ground_text_4;
    public TextMeshProUGUI score_text_4;
    public Text player_4_id;

    // public GameObject player_pannel_1, player_pannel_2, player_pannel_3, player_pannel_4;   

    //evnetmanager ==> 좌표로 차지한 땅수 표현
    //networkroundmanger ==> observer로 변경 후 가져가서 사용
    // 점수는 나중에!

    // Start is called before the first frame update
    void Start()
    {
        Player_Id = NetworkRoundManager.public_Player_Id;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(NetworkRoundManager.player_Number);
        switch (NetworkRoundManager.player_Number)
        {
            case 2:
                {
                    Player_pannel_3.SetActive(false);
                    Player_pannel_4.SetActive(false);
                    
                    player_1_id.text = "1 Player";//추후 닉네임변경
                    player_2_id.text = "2 Player";//추후 닉네임변경

                    if (NetworkRoundManager.nowRound >= 1)
                    {
                        //EventManager.player_count[0];
                        //EventManager.player_count[1]; //2플레이어 점령땅수
                        player_1_card = NetworkRoundManager.cardNum[0];
                        player_2_card = NetworkRoundManager.cardNum[1];


                        //player_pannel_1.GetComponentInChildren<>
                        //GameObject player_1_txt = player_pannel_1.transform.GetChild(2).GetComponent<Text>;
                        //GameObject player_2_txt = player_pannel_2.transform.GetChild(2).GetChild(0).gameObject;

                        card_text_1.text = player_1_card.ToString();
                        card_text_2.text = player_2_card.ToString();
                    }
                    break;
                }
            case 3:
                {
                    break;
                }
            case 4:
                {
                    break;
                }
        }

    }
}
/*
     *  플레이어 id =1 --> 234 
     *  플레이어 id =2 --> 134
     *  플레이어 id =3 --> 124
     *  플레이어 id =4 --> 123
     * 
     *  각 플레이어에 따라 resource에 이미지 불러서 입히기
     *  필요한것 카드개수, 점령땅 개수, 총점수(미구현)
     *  
     * 
*/
