using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class NetworkRoundManager : MonoBehaviourPunCallbacks, IPunObservable
{
    PhotonView pv;

    public GameObject flag_1, flag_2, flag_3, flag_4;
    public int startPlayerId; //현재 라운드의 선 플레이어
    public int inRoundingPlayerId; //현재 라운드의 턴을 진행하고 있는 플레이어
    public int nowRound = 0; // 라운드
    private int myPlayerId; 
    public static int public_Player_Id;
    public static int public_nowRound;

    public TextMeshProUGUI roundText;
    public TextMeshProUGUI TrunText;
    public Button endButton;
    public Button selectButton;

    private List<bool> playerTrun = new List<bool>(); //해당 라운드에서 플레이어턴이 끝났는지 확인 모든 값이 false이면 다음 라운드로 진행
    private bool cardDraw = false;
    public static List<int> cardNum = new List<int>();

    //Select Button 클릭시 사용한 카드 ID와 좌표값을 EventManager 타일변수에 입력
    public static int? selectCard = null;
    public static int locX, locY;
    
    void Awake()
    {
        pv = this.GetComponent<PhotonView>();

        //최초 선플레이어 지정, 모든 유저의 Turn을 True 플레이어 Id값 입력
        if (PhotonNetwork.IsMasterClient)
        {
            startPlayerId = Random.Range(0, PhotonNetwork.PlayerList.Length);
            inRoundingPlayerId = startPlayerId;
        }
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            playerTrun.Add(true);
        }
        myPlayerId = PhotonNetwork.LocalPlayer.ActorNumber;
        public_Player_Id = myPlayerId;
    }

    void FixedUpdate()
    {
        
        Debug.Log($"방에 참가한 인원 수 : {PhotonNetwork.PlayerList.Length}");
        Debug.Log($"나의 PlayerId : {myPlayerId}");
        Debug.Log($"현재 라운드 선플레이어 : {startPlayerId + 1}");

        //라운드 종료함수 호출
        if (PhotonNetwork.IsMasterClient)
        {
            if (!playerTrun.Contains(true))
            {
                Debug.Log("라운드종료 함수 호출");
                RPCRoundEnd();
            }
        }

        //해당 턴 플레이어만 버튼 활성화
        if (myPlayerId == inRoundingPlayerId + 1)
        {
            endButton.interactable = true;
            //카드, 타일 선택이 모두 완료되면 셀렉트버튼 활성화
            if (selectCard.HasValue && MouseScripts.choice_Map == true) { selectButton.interactable = true; }
            else { selectButton.interactable = false; }
        }
        else
        {
            endButton.interactable = false;
            selectButton.interactable = false;
        }

        if(nowRound != 0 && cardDraw == false && GameObject.Find("Hand").transform.childCount < 2)
        {
            cardDraw = true;
            if(!PhotonNetwork.IsMasterClient) pv.RPC("requestCardDraw", RpcTarget.MasterClient);
        }

        //UI 스크립트
        TrunText.text = $"{inRoundingPlayerId + 1}player Turn";
        roundText.text = $"{nowRound} Round";
    }

    #region RPC함수
    
    [PunRPC]
    private void RPCEndPlayer(int plyerId)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            playerTrun[plyerId - 1] = false;
            inRoundingPlayerId = (inRoundingPlayerId + 1) % PhotonNetwork.PlayerList.Length;
        }
    }

    [PunRPC]
    private void RPCNextPlayer()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            inRoundingPlayerId = (inRoundingPlayerId + 1) % PhotonNetwork.PlayerList.Length;
            if (!playerTrun.Contains(true)) return;
            if (playerTrun[inRoundingPlayerId] == false) RPCNextPlayer();
        }
    }

    [PunRPC]
    private void CardDisStart() => GameObject.Find("DeckSystem").GetComponent<CardDistribute>().AllPlayerCardDistribute();

    [PunRPC]
    private void requestCardDraw() => cardDraw = true;

    [PunRPC]
    private void RPCRoundEnd()
    {
        //Debug.Log(GameObject.Find("Hand").transform.childCount);
        if (cardDraw)
        {
            pv.RPC("CardDisStart", RpcTarget.MasterClient);
        }
        if (PhotonNetwork.IsMasterClient)
        {
            startPlayerId = (startPlayerId + 1) % PhotonNetwork.PlayerList.Length;
            inRoundingPlayerId = startPlayerId;
            nowRound += 1;
            for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
            {
                playerTrun[i] = true;
            }
        }
    }

    [PunRPC]
    private void RPCCardUseData(int locX, int locY, int playerId, int value)
    {
        //playerId, cardId
        EventManager.selectTiles[locX][locY].Add(new Tiles(playerId, value));
        EventManager.occTiles[locX,locY] = playerId;
        EventManager.tileLocX.Add(locX);
        EventManager.tileLocY.Add(locY);

        
        //좌표 읽어오기
        Debug.Log("choice");
        GameObject go = GameObject.Find(locX + ", " + locY);
        MeshRenderer mr = go.GetComponent<MeshRenderer>();
        float pos_x = go.transform.position.x;
        float pos_z = go.transform.position.z;

        switch (playerId)
        {
            case 1:
                {
                    Instantiate(flag_1, new Vector3(pos_x, 1, pos_z), flag_1.transform.rotation);
                    mr.material.color = new Color(255 / 255f, 0, 0);
                    break;
                }
            case 2:
                {
                    Instantiate(flag_2, new Vector3(pos_x, 1, pos_z), flag_2.transform.rotation);
                    mr.material.color = new Color(83 / 255f, 147 / 255f, 224 / 255f);
                    break;
                }
            case 3:
                {
                    Instantiate(flag_3, new Vector3(pos_x, 1, pos_z), flag_3.transform.rotation);
                    mr.material.color = new Color(248 / 255f, 215 / 255f, 0);
                    //mr.material = Resources.Load("player_choice_yellow", typeof(Material)) as Material;
                    break;
                }
            case 4:
                {
                    Instantiate(flag_4, new Vector3(pos_x, 1, pos_z), flag_4.transform.rotation);
                    mr.material.color = new Color(168 / 255f, 0 / 255f, 255 / 255f);
                    //mr.material = Resources.Load("player_choice_yellow", typeof(Material)) as Material;
                    break;
                }
        }
        
    }

    #endregion

    #region 버튼 및 함수부
    public void EndTrun()
    {
        playerTrun[myPlayerId - 1] = false;
        if (!PhotonNetwork.IsMasterClient)
        {
            Debug.Log("send Master next Trun");
            pv.RPC("RPCEndPlayer", RpcTarget.MasterClient, myPlayerId);
        }
        else
        {
            inRoundingPlayerId = (inRoundingPlayerId + 1) % PhotonNetwork.PlayerList.Length;
        }
    }

    public void SelectButton()
    {
        //selectCAndT[0, 1], [0, 2] ==> x,y좌표
        Debug.Log("선택된 맵 x , y좌표 = " + "(" + MouseScripts.choice_Map_x + "," + MouseScripts.choice_Map_y + ")");

        //selectTiles[x][y].Add( playerId, cardid);
        locX = MouseScripts.choice_Map_x;
        locY = MouseScripts.choice_Map_y;

        MouseScripts.mr.material.color = new Color(32 / 255f, 84 / 255f, 30 / 255f);
        MouseScripts.choice_Map = false;
        MouseScripts.ps.Stop();
        MouseScripts.ps.Clear();

        //playerId, cardId
        pv.RPC("RPCCardUseData", RpcTarget.AllBuffered, locX, locY, myPlayerId, selectCard.Value);

        GameObject destoy_Card = GameObject.FindGameObjectWithTag("UseCard");
        Destroy(destoy_Card);

        pv.RPC("RPCNextPlayer", RpcTarget.MasterClient);
        selectCard = null;
    }
    #endregion

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(startPlayerId);
            stream.SendNext(inRoundingPlayerId);
            stream.SendNext(nowRound);
        }
        else
        {
            startPlayerId = (int)stream.ReceiveNext();
            inRoundingPlayerId = (int)stream.ReceiveNext();
            nowRound = (int)stream.ReceiveNext();
        }
    }
}
