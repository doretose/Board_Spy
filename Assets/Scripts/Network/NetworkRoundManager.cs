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

    //UI 및 리소스
    public GameObject flag_1, flag_2, flag_3, flag_4;
    public TextMeshProUGUI roundText;
    public TextMeshProUGUI TrunText;
    public Button endButton;
    public Button selectButton;

    //플레이어의 정체성
    private int myPlayerId; 
    public static int public_Player_Id;
    public static int public_nowRound;

    //호스트가 관리하고 각 클라이언트에게 공유되는 변수들
    public int startPlayerId; //현재 라운드의 선 플레이어
    public int inRoundingPlayerId; //현재 라운드의 턴을 진행하고 있는 플레이어
    public int nowRound = 0; // 라운드

    //호스트만 관리하는 변수들
    private List<bool> playerTrun = new List<bool>(); //해당 라운드에서 플레이어턴이 끝났는지 확인 모든 값이 false이면 다음 라운드로 진행
    public static List<int> cardNum = new List<int>(); //각 플레이어의 핸드 수를 확인 [0 = 1player ... 3 = 4player]

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
            for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
            {
                playerTrun.Add(true);
                cardNum.Add(0);
            }
        }
        myPlayerId = PhotonNetwork.LocalPlayer.ActorNumber;
        public_Player_Id = myPlayerId;
    }

    void FixedUpdate()
    {
        Debug.Log($"방에 참가한 인원 수 : {PhotonNetwork.PlayerList.Length}");
        Debug.Log($"나의 PlayerId : {myPlayerId}");
        Debug.Log($"현재 라운드 선플레이어 : {startPlayerId + 1}");
        

        //라운드 종료함수 호출(호스트만)
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log($"현재 카드수 1 : {cardNum[0]}, 2 : {cardNum[1]}, 3 :  4 : ");
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

        //UI 스크립트
        TrunText.text = $"{inRoundingPlayerId + 1}player Turn";
        roundText.text = $"{nowRound} Round";
    }

    #region RPC함수

    //버튼 이벤트 EndTurn 호스트에게만 => RPCEndPlayer()
    //해당 player의 Trun상황을 호스트가 False로 변경해주고 다음 플레이어로 변경
    [PunRPC]
    private void RPCEndPlayer(int playerId)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            playerTrun[playerId - 1] = false;
            inRoundingPlayerId = (inRoundingPlayerId + 1) % PhotonNetwork.PlayerList.Length;
            if (!playerTrun.Contains(true)) return; //
            if (playerTrun[inRoundingPlayerId] == false) RPCNextPlayer();
        }
    }

    //버튼 이벤트 selectButton 호스트에게만 => RPCNextPlayer()
    //해당 player의 Trun상황을 변경하지않고 순서만 넘기는 스크립트
    [PunRPC]
    private void RPCNextPlayer()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            inRoundingPlayerId = (inRoundingPlayerId + 1) % PhotonNetwork.PlayerList.Length;
            if (!playerTrun.Contains(true)) return; //
            if (playerTrun[inRoundingPlayerId] == false) RPCNextPlayer();
        }
    }

    //모든 플레이어의 Trun 상황이 false라면 시작된다. Update에 위치
    //각 플레이어의 카드 수를 확인하고 2장 미만이라면 드로우 수행 => CardDisStart() => AllPlayerCardDistribute() => (RPC.All)RPCCardDistribute => (코루틴)카드 배부
    //선플레이어를 다음플레이어로 변경하고 Round를 증가, 모든 플레이어의 Trun상황을 True로 변경 
    [PunRPC]
    private void RPCRoundEnd()
    {
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            if(cardNum[i] < 2)
            {
                CardDisStart();
                break;
            }
        }
        startPlayerId = (startPlayerId + 1) % PhotonNetwork.PlayerList.Length;
        inRoundingPlayerId = startPlayerId;
        nowRound += 1;
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            playerTrun[i] = true;
        }
    }

    //카드를 타일에 사용하는 함수
    //selectButton 모든 플레이어에게 => RPCCardUseData()
    //EventManager의 타일관련 함수에 카드의 데이터와 타일의 데이터를 입력한다.
    //모든 플레이어가 각자의 원장을 가지고 있다고 생각하자
    [PunRPC]
    private void RPCCardUseData(int locX, int locY, int playerId, int value)
    {
        if(PhotonNetwork.IsMasterClient) cardNum[playerId - 1] -= 1;
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
        endButton.interactable = false;
        if (!PhotonNetwork.IsMasterClient)
        {
            Debug.Log("send Master next Trun");
            pv.RPC("RPCEndPlayer", RpcTarget.MasterClient, myPlayerId);
        }
        else
        {
            inRoundingPlayerId = (inRoundingPlayerId + 1) % PhotonNetwork.PlayerList.Length;
            playerTrun[myPlayerId - 1] = false;
        }
    }

    public void SelectButton()
    {
        selectButton.interactable = false;
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

    private void CardDisStart()
    {
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            cardNum[i] += 5;
        }
        GameObject.Find("DeckSystem").GetComponent<CardDistribute>().AllPlayerCardDistribute();
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
