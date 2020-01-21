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
    //public GameObject flag_1, flag_2, flag_3, flag_4;
    public GameObject[] tokken = new GameObject[4];
    public GameObject[] castle = new GameObject[4];
    public GameObject sword_ani;
    public GameObject[] endTurnText = new GameObject[4];
    public GameObject[] Crown = new GameObject[4];
    private static Color[] myColor = new Color[5] { new Color(32 / 255f, 84 / 255f, 30 / 255f), new Color(255 / 255f, 0, 0) , new Color(83 / 255f, 147 / 255f, 224 / 255f), new Color(248 / 255f, 215 / 255f, 0), new Color(168 / 255f, 0 / 255f, 255 / 255f) };
    public TextMeshProUGUI roundText;
    public TextMeshProUGUI TurnText;
    public TextMeshProUGUI actTxt;
    public Button endButton;
    public Button selectButton;
    public Button baseSelectButton;
    public GameObject choice_effect;

    //플레이어의 정체성
    private int myPlayerId; 
    public static int public_Player_Id;
    public static int public_nowRound;
    public static int player_Number;

    //호스트가 관리하고 각 클라이언트에게 공유되는 변수들
    public int startPlayerId; //현재 라운드의 선 플레이어
    public int inRoundingPlayerId; //현재 라운드의 턴을 진행하고 있는 플레이어
    public static int nowRound = 0; // 라운드

    //호스트만 관리하는 변수들
    private List<bool> playerTurn = new List<bool>(); //해당 라운드에서 플레이어턴이 끝났는지 확인 모든 값이 false이면 다음 라운드로 진행
    public static List<int> cardNum = new List<int>(); //각 플레이어의 핸드 수를 확인 [0 = 1player ... 3 = 4player]

    //Select Button 클릭시 사용한 카드 ID와 좌표값을 EventManager 타일변수에 입력
    public static int? selectCard = null;
    private int locX, locY;

    //시간제한 함수
    public TextMeshProUGUI timeText;
    private float timeCost;

    //라운드실행제어 변수
    public static bool roundProcessBool;
    public static bool isMyTurn = false;
    public static int roundLimit;

    public GameObject[] player_pannel_bg = new GameObject[4];
    void Awake()
    {
        //초기화
        nowRound = 0;
        playerTurn.Clear();
        cardNum.Clear();
        roundProcessBool = false;
        roundLimit = 2;

        pv = this.GetComponent<PhotonView>();

        player_Number = PhotonNetwork.PlayerList.Length;

        //최초 선플레이어 지정, 모든 유저의 Turn을 True 플레이어 Id값 입력
        if (PhotonNetwork.IsMasterClient)
        {
            startPlayerId = Random.Range(0, player_Number);
            inRoundingPlayerId = startPlayerId;

        }
        for (int i = 0; i < player_Number; i++)
        {
            playerTurn.Add(true);
            cardNum.Add(0);
        }

        myPlayerId = PhotonNetwork.LocalPlayer.ActorNumber;
        public_Player_Id = myPlayerId;
        timeCost = 20;
    }

    void FixedUpdate()
    {
        //라운드 결과 처리
        if (roundProcessBool)
        {
            RoundResultProcessIng();
            return;
        }

        Debug.Log($"현재 내 Id : {myPlayerId}");

        //Text UI 스크립트
        if (inRoundingPlayerId + 1 == myPlayerId)
        {
            actTxt.color = getMyColor(myPlayerId);
            actTxt.text = "Your Turn";
            TurnText.color = getMyColor(myPlayerId);
            TurnText.text = "My Turn";
        }
        else if (PhotonNetwork.PlayerList.Length > 1)
        {
            actTxt.color = Color.black;
            TurnText.color = Color.black;
            actTxt.text = $"{PhotonNetwork.PlayerList[inRoundingPlayerId].NickName} Turn";
            TurnText.text = $"{inRoundingPlayerId + 1}player Turn";
        }
        

        // Debug.Log($"My Player Id : {myPlayerId}");
        //라운드 종료함수 호출(호스트만)
        if (PhotonNetwork.IsMasterClient)
        {
            if (!playerTurn.Contains(true))
            {
                Debug.Log("라운드종료 함수 호출");
                MasterRoundEnd();
            }
        }

        if (nowRound < roundLimit)
        {
            roundText.text = $"{nowRound} Round";
        }
        else if (nowRound >= roundLimit)
        {
            roundText.text = $"Last Round";
            roundText.color = Color.red;
        }

        //플레이어 패널 백그라운드, 플레이어 턴 종료, 크라운 활성
        for (int i = 0; i < player_Number; i++)
        {
            if (i == inRoundingPlayerId) player_pannel_bg[i].SetActive(true);
            else player_pannel_bg[i].SetActive(false);

            if (playerTurn[i]) endTurnText[i].SetActive(false);
            else endTurnText[i].SetActive(true);

            if (i == startPlayerId) Crown[i].SetActive(true);
            else Crown[i].SetActive(false);
        }

        //베이스 캠프 선택 0라운드
        if (nowRound == 0)
        {
            RoundZeroAction();
            return;
        }

        //해당 턴 플레이어만 버튼 활성화
        if (myPlayerId == inRoundingPlayerId + 1)
        {
            isMyTurn = true;
            endButton.interactable = true;
            TimeCount();//시간제한 함수 실행
            //카드, 타일 선택이 모두 완료되면 셀렉트버튼 활성화
            if (selectCard.HasValue && MouseScripts.choice_Map == true) { selectButton.interactable = true; }
            else { selectButton.interactable = false; }
        }
        else
        {
            isMyTurn = false;
            //시간제한
            timeText.gameObject.SetActive(false);
            timeCost = 20;

            endButton.interactable = false;
            selectButton.interactable = false;
        }       
    }

    #region 버튼 및 함수부
    public void EndTurn()
    {
        endButton.interactable = false;
        timeCost = 20;
        if (MouseScripts.mr.material.color != null)
            init_tile(MouseScripts.choice_Map_x, MouseScripts.choice_Map_y);

        if (!PhotonNetwork.IsMasterClient)
        {
            pv.RPC("RPCEndPlayer", RpcTarget.MasterClient, myPlayerId);
        }
        else
        {
            RPCEndPlayer(myPlayerId);
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

        init_tile(locX, locY);

        //playerId, cardId
        pv.RPC("RPCCardUseData", RpcTarget.AllBuffered, locX, locY, myPlayerId, selectCard.Value);

        GameObject destoy_Card = GameObject.FindGameObjectWithTag("UseCard");
        Destroy(destoy_Card);

        pv.RPC("RPCNextPlayer", RpcTarget.AllBuffered);
        timeCost = 20;
        selectCard = null;
    }

    void init_tile(int locX, int locY)
    {
        int occTileId = GameObject.Find("EventSystem").GetComponent<EventManager>().getOccTiles(locX, locY);
        MouseScripts.mr.material.color = myColor[occTileId];
        MouseScripts.choice_Map = false;
        MouseScripts.ps.Stop();
        MouseScripts.ps.Clear();
    }

    //베이스캠프 셀렉트 버트 클릭시 실행 0라운드에 진행되는 베이스캠프 선택
    public void SelectBaseButton()
    {
        baseSelectButton.interactable = false;
        //맵 선택하면 Button 활성화하고 해당 맵의 좌표를 EvenetManager의 occTiles에 넣어준다.
        locX = MouseScripts.choice_Map_x;
        locY = MouseScripts.choice_Map_y;

        int occTileId = GameObject.Find("EventSystem").GetComponent<EventManager>().getOccTiles(locX, locY);
        MouseScripts.mr.material.color = myColor[occTileId];
        MouseScripts.choice_Map = false;
        MouseScripts.ps.Stop();
        MouseScripts.ps.Clear();

        pv.RPC("RPCSelectBase", RpcTarget.AllBuffered, locX, locY, myPlayerId);

        //버튼 파괴하고 정상작동하도록
        Destroy(baseSelectButton.gameObject);
        EndTurn();
    }

    //모든 플레이어의 Trun 상황이 false라면 시작된다. Update에 위치
    //각 플레이어의 카드 수를 확인하고 2장 미만이라면 드로우 수행 => CardDisStart() => AllPlayerCardDistribute() => (RPC.All)RPCCardDistribute => (코루틴)카드 배부
    //선플레이어를 다음플레이어로 변경하고 Round를 증가, 모든 플레이어의 Trun상황을 True로 변경 
    private void MasterRoundEnd()
    {
        //for (int i = 0; i < player_Number; i++)
        //{
        //    playerTurn[i] = true;
        //    player_pannel_bg[i].SetActive(false);
        //    endTurnText[i].SetActive(true);
        //}

        //라운드 처리 관련 함수 호출, roundProcessBool로 해당 스크립트 통제
        pv.RPC("RPCRoundEnd", RpcTarget.AllBuffered);

        for (int i = 0; i < player_Number; i++)
        {
            if (cardNum[i] < 2)
            {
                CardDisStart();
                break;
            }
        }

        startPlayerId = (startPlayerId + 1) % player_Number;
        inRoundingPlayerId = startPlayerId;
        //nowRound += 1;
    }

    private void CardDisStart()
    {
        for (int i = 0; i < player_Number; i++)
        {
            cardNum[i] += 5;
        }
        GameObject.Find("DeckSystem").GetComponent<CardDistribute>().AllPlayerCardDistribute();
    }

    //시간제한 활성함수 자신의 턴일 경우 해당 함수를 진행한다.
    private void TimeCount()
    {
        timeText.gameObject.SetActive(true);
        timeCost -= Time.deltaTime;
        timeText.text = string.Format("{0:0}", timeCost);

        if (timeCost < 0.0f)
        {
            EndTurn();
            timeText.gameObject.SetActive(false);
            return;
        }
    }

    //0라운드 베이스캠프 선택동안 UPDATE 제어
    private void RoundZeroAction()
    {
        TurnText.text = $"{inRoundingPlayerId + 1} BaseCamp";
        endButton.interactable = false;
        selectButton.interactable = false;
        timeText.gameObject.SetActive(false);

        if (myPlayerId == inRoundingPlayerId + 1)
        {
            isMyTurn = true;
            if (MouseScripts.choice_Map == true) baseSelectButton.interactable = true;
            else baseSelectButton.interactable = false;
        } else
        {
            isMyTurn = false;
        }
    }

    private void RoundResultProcessIng()
    {
        TurnText.color = Color.black;
        TurnText.text = $"Round Result";
        endButton.interactable = false;
        selectButton.interactable = false;
        timeText.gameObject.SetActive(false);
        timeCost = 20;
    }

    public static Color getMyColor(int occId)
    {
        return myColor[occId];
    }

    #endregion

    #region RPC함수
    //버튼 이벤트 EndTurn 호스트에게만 => RPCEndPlayer()
    //해당 player의 Trun상황을 호스트가 False로 변경해주고 다음 플레이어로 변경
    [PunRPC]
    private void RPCEndPlayer(int playerId)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            playerTurn[playerId - 1] = false;
            inRoundingPlayerId = (inRoundingPlayerId + 1) % player_Number;
            if (!playerTurn.Contains(true)) return; //
            if (playerTurn[inRoundingPlayerId] == false)
            {
                RPCNextPlayer();
                return;
            }
            pv.RPC("StartActiveTxt", RpcTarget.AllBuffered);
        }
    }

    //버튼 이벤트 selectButton 호스트에게만 => RPCNextPlayer()
    //해당 player의 Trun상황을 변경하지않고 순서만 넘기는 스크립트
    [PunRPC]
    private void RPCNextPlayer()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            inRoundingPlayerId = (inRoundingPlayerId + 1) % player_Number;
            if (!playerTurn.Contains(true)) return; //
            if (playerTurn[inRoundingPlayerId] == false)
            {
                RPCNextPlayer();
                return;
            }
            pv.RPC("StartActiveTxt", RpcTarget.AllBuffered);
        }
    }

    [PunRPC]
    private void StartActiveTxt()
    {
        GameObject.Find("EventSystem").GetComponent<EventManager>().TweenMoveObj();
    }
   
    //카드를 타일에 사용하는 함수
    //selectButton 모든 플레이어에게 => RPCCardUseData()
    //EventManager의 타일관련 함수에 카드의 데이터와 타일의 데이터를 입력한다.
    //모든 플레이어가 각자의 원장을 가지고 있다고 생각하자
    [PunRPC]
    private void RPCCardUseData(int locX, int locY, int playerId, int value)
    {
        bool anotherCard;
        if(PhotonNetwork.IsMasterClient) cardNum[playerId - 1] -= 1;
        //playerId, cardId
        anotherCard = GameObject.Find("EventSystem").GetComponent<EventManager>().setSelectTiles(locX, locY, playerId, value);
        if(anotherCard)
        {
            EventManager.tileLocX.Add(locX);
            EventManager.tileLocY.Add(locY);
        }
        //17일 추가
        GameObject endTile = GameObject.Find(locX + ", " + locY);
        Transform[] endTileChilds;
        endTileChilds = endTile.GetComponentsInChildren<Transform>(true);
        
        if (endTileChilds.Length < 8)
        {
            PrefebManager.DestroyPrefebs(locX, locY);
            PrefebManager.CreatePrefeb(tokken[playerId - 1], locX, locY, choice_effect);
        }
        else {
            PrefebManager.DestroyPrefebs(locX, locY);
            PrefebManager.CreateSwordPrefeb(sword_ani, locX, locY, choice_effect);
        }
    }

    [PunRPC]
    private void RPCRoundEnd()
    {
        roundProcessBool = true;

        for (int i = 0; i < player_Number; i++)
        {
            playerTurn[i] = true;
            player_pannel_bg[i].SetActive(false);
            endTurnText[i].SetActive(true);
        }

        GameObject.Find("EventSystem").GetComponent<EventManager>().StartRoundResult();
    }

    //0라운드 베이스 선택관련 RPC함수 모든 인원들은 해당 플레이어가 어디로 베이스 캠프를 선정했는지 확인한다.
    [PunRPC]
    private void RPCSelectBase(int locX, int locY, int playerId)
    {
        GameObject.Find("EventSystem").GetComponent<EventManager>().setOccTiles(locX, locY, playerId);

        GameObject go = GameObject.Find(locX + ", " + locY);
        go.GetComponent<Hex>().thisBaseCamp = true;

        //해당 타일에 베이스캠프 이미지를 구현
        PrefebManager.CreatePrefeb(castle[playerId - 1], locX, locY, myColor[playerId]);
    }

    #endregion

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(startPlayerId);
            stream.SendNext(inRoundingPlayerId);
            for (int i = 0; i < player_Number; i++)
            {
                stream.SendNext(playerTurn[i]);
                stream.SendNext(cardNum[i]);
            }
        }
        else
        {
            startPlayerId = (int)stream.ReceiveNext();
            inRoundingPlayerId = (int)stream.ReceiveNext();
            for (int i = 0; i < player_Number; i++)
            {
                playerTurn[i] = (bool)stream.ReceiveNext();
                cardNum[i] = (int)stream.ReceiveNext();
            }
        }
    }
}