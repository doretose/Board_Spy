using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class player_pannel : MonoBehaviour
{
    public int Player_Id;
    int player_1, player_2, player_3, player_4;
    public GameObject player_pannel_1, player_pannel_2, player_pannel_3, player_pannel_4;   
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
        switch (NetworkManager.Player_Count)
        {
            case 2:
                {
                    player_1 = EventManager.player_count[0];
                    player_2 = EventManager.player_count[1];
                    //player_pannel_1.GetComponentInChildren<>

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
