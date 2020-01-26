using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EventManager : MonoBehaviour
{
    #region 타일관련 변수
    //타일크기 관련 변수
    private static readonly int Xscale = 15;
    private static readonly int Yscale = 8;

    //이벤트를 처리해야할 내용을 다음 selectTiles와 최종 점령지를 나타내고 있는 occtiles
    /*
     * 01-22 일단 public 추후 다른방법 필요
     * */
    public static List<List<List<Tiles>>> selectTiles = new List<List<List<Tiles>>>();
    
    private int[,] occTiles = new int[Xscale, Yscale];
    private bool[,] occTilesVisit = new bool[Xscale, Yscale]; //false

    //타일 2차원배열에 접근하기 위해서 현재 지정된 좌표가 어디인지 확인하기 위해서 선택된 X,Y좌표를 저장
    public static List<int> tileLocX = new List<int>();
    public static List<int> tileLocY = new List<int>();

    //정보 기록 변수
    public static int[] player_count = new int[4] { 0, 0, 0, 0 }; //점령지 수
    public static int[] player_score = new int[4] { 0, 0, 0, 0 }; //플레이어별 점수

    //타일 순회를 위한 x,y좌표 저장 변수
    private int LocX, LocY;
    //주변 타일 (좌상, 좌하, 우상, 우하, 좌, 우 Y좌표가 홀수일 때 이대로 짝수일 때는 0~3번인덱스 -1 ) 
    private Pair<int, int>[] loc = new Pair<int, int>[6]
        { new Pair<int, int>(0,1), new Pair<int, int>(0,-1), new Pair<int, int>(1,1), new Pair<int, int>(1,-1), new Pair<int, int>(-1,0), new Pair<int, int>(+1,0) };

    //턴 처리 종료시 다음 턴 진행인원에게 턴 안내
    public GameObject activeTextObj;
    private Vector3 initPos; //턴 안내 텍스트 최초 위치 저장

    //깃발 프리팹
    public GameObject[] flag = new GameObject[4];

    //턴끝날때 화살표 프리팹
    public GameObject arrow_tile2;
    #endregion

    //플레이어 패널
    public GameObject game_result_pannel;
    public GameObject gameEndPannel;
    public GameObject arrow_tile;
    // 3차원 배열 selectTiles 초기화
    private void Awake()
    {
        player_count = new int[4] { 0, 0, 0, 0 };
        player_score = new int[4] { 0, 0, 0, 0 };

        for (int i = 0; i < Xscale; i++)
        {
            selectTiles.Add(new List<List<Tiles>>());
            for (int j = 0; j < Yscale; j++)
            {
                selectTiles[i].Add(new List<Tiles>());
            }
        }

        initPos = activeTextObj.transform.position;
    }

    public void StartRoundResult()
    {
        StartCoroutine("RoundResult");
    }

    // 타일 위치에 관한 이벤트 처리
    IEnumerator RoundResult()
    {
        int occPlayerId, checkPlayer, checkCard;
        List<Tiles> resultTiles = new List<Tiles>();

        //실행을 받으면 타일 하나씩 순회하면서 해당 타일의 최종 점령 playerId를 확인한다.
        //종료시점에 occPlayerId에 playerId를 넣고 occTile에 occPlayerId값을 입력, 종료시 타일 clear()
        for (int i = 0; i < tileLocX.Count; ++i)
        {
            resultTiles.Clear();
            occPlayerId = 0;
            LocX = tileLocX[i];
            LocY = tileLocY[i];
            
            Debug.Log("<color=red>Prefab create : </color>" + LocX +", " + LocY);
            PrefebManager.CreateArrowPrefeb(arrow_tile, LocX, LocY);
            yield return new WaitForSeconds(2);
            MouseScripts.result_pannel_roundover(LocX,  LocY);
            for (int j = 0; j < selectTiles[LocX][LocY].Count; ++j)
            {
                checkPlayer = selectTiles[LocX][LocY][j].playerId;
                checkCard = selectTiles[LocX][LocY][j].cardId;

                //사용 카드 처리
                switch (checkCard)
                {
                    case 1: //점령
                        {
                            resultTiles.Add(new Tiles(checkPlayer, checkCard));
                            break;
                        }
                    case 2: //방어
                        {
                            //방어카드를 사용한 플레이어의 땅이라면?
                            if(occTiles[LocX, LocY] == checkPlayer)
                            {
                                int idx = 0;
                                while (idx < resultTiles.Count)
                                {
                                    if (resultTiles[idx].playerId != checkPlayer)
                                    {
                                        Debug.Log("삭제! " + resultTiles[idx].playerId);
                                        resultTiles.RemoveAt(idx);
                                        break;
                                    }
                                    ++idx;
                                }
                            }
                            break;
                        }
                    case 3: //매수
                        {
                            int idx = 0;
                            while (idx < resultTiles.Count)
                            {
                                if (resultTiles[idx].cardId == 1) resultTiles.RemoveAt(idx);
                                else ++idx;
                            }
                            resultTiles.Add(new Tiles(checkPlayer, checkCard));
                            break;
                        }
                    default: //블러핑
                            break;
                }
            }
            yield return new WaitForSeconds(4);
            MouseScripts.war_result_off();
            //탐색이 끝난 타일 초기화
            selectTiles[LocX][LocY].Clear();

            //프리팹 삭제
            PrefebManager.DestroyPrefebs(LocX, LocY);
            
            //변경값이 있는가
            if (resultTiles.Count > 0)
            {
                Debug.Log($"({LocX} , {LocY}) = {resultTiles[0].playerId}");
                occPlayerId = resultTiles[0].playerId;
            }

            //해당 타일 프리팹 생성
            if (occPlayerId != 0)
            {
                occTiles[LocX, LocY] = occPlayerId;
                PrefebManager.CreatePrefeb(flag[occPlayerId - 1], LocX, LocY, NetworkRoundManager.getMyColor(occPlayerId));
            } else if(occTiles[LocX, LocY] != 0)
            {
                int tempid = occTiles[LocX, LocY];
                PrefebManager.CreatePrefeb(flag[tempid - 1], LocX, LocY, NetworkRoundManager.getMyColor(tempid));
            }
            yield return new WaitForSeconds(2);
        }

        //최종적으로 player_count를 재탐색하고 타일 초기화 -> occTile에 따른 결과 반영
        tileLocX.Clear();
        tileLocY.Clear();

        //라운드결과 종료 => 점령지, 점수 계산
        RoundResultEnd();
        
        NetworkRoundManager.nowRound += 1;

        if (NetworkRoundManager.nowRound > NetworkRoundManager.roundLimit)
        {
            yield return new WaitForSeconds(2);

            game_result_pannel.SetActive(true);
        }
        else
        {
            yield return new WaitForSeconds(1);

            //라운드 텍스트 이동
            NetworkRoundManager.roundProcessBool = false;
            TweenMoveObj();
        }
    }
    //패널 결과 처리 함수
    private void round_end_pannel()
    {
        /* 라운드가 끝날때 카드가 한개이상 사용된 땅에
         *  어떤 플레이어가 어떠한 카드를 사용했는지 보여줌
         * */
    }
    //라운드결과 종료
    private void RoundResultEnd()
    {
        player_count = new int[4] { 0, 0, 0, 0 };
        player_score = new int[4] { 0, 0, 0, 0 };

        for (int i = 0; i < occTiles.GetLength(0); i++)
        {
            for (int j = 0; j < occTiles.GetLength(1); j++)
            {
                if (occTiles[i, j] != 0)
                {
                    player_count[occTiles[i, j] - 1] += 1;
                    occTilesVisit[i, j] = true;
                }
                else { occTilesVisit[i, j] = false; }
            }
        }
        //점수 확인
        Score_check();
    }

    //점수 계산
    private void Score_check()
    {
        //타일 좌표를 전체 탐색 
        //좌표에 해당하는 occTiles가 0이 아니고 occTilesVisitVisit true라면 ScoreTilesVisit 호출
        for (int i = 0; i < occTiles.GetLength(0); ++i)
        {
            for (int j = 0; j < occTiles.GetLength(1); ++j)
            {
                if (occTiles[i, j] != 0 && occTilesVisit[i, j] == true)
                {
                    player_score[occTiles[i, j] - 1] += (int)Math.Pow(2,ScoreTilesVisit(i,j, occTiles[i, j]) - 1);
                }
            }
        }
    }

    private int ScoreTilesVisit(int x, int y, int playerId)
    {
        int minusEven = 0;
        int result = 0;
        if(x < 0 || x >= Xscale || y < 0 || y >= Yscale || playerId != occTiles[x, y]) return 0;

        if (occTilesVisit[x, y] == true)
        {
            occTilesVisit[x, y] = false;
            result += 1;
        }
        else return 0;


        if ((y % 2) == 0) minusEven = 1;

        for(int i = 0; i < loc.Length; ++i)
        {
            if (i > 3) minusEven = 0;
            result += ScoreTilesVisit(x + (loc[i].First - minusEven), y + loc[i].Second, playerId);
        }

        return result;
    }

    public void TweenMoveObj()
    {
        StartCoroutine("TweenMove");
    }

    IEnumerator TweenMove()
    {
        activeTextObj.transform.position = initPos;
        iTween.MoveTo(activeTextObj, iTween.Hash("islocal", true, "x", 0, "time", 0.8f, "easetype", iTween.EaseType.easeOutQuint));
        yield return new WaitForSeconds(0.8f);
        iTween.MoveTo(activeTextObj, iTween.Hash("islocal", true, "x", -700, "time", 0.6f, "easetype", iTween.EaseType.easeInQuint));
        yield return null;
    }

    public int getOccTiles(int locX, int locY)
    {
        return occTiles[locX, locY];
    }

    public void setOccTiles(int locX, int locY, int playerId) => occTiles[locX, locY] = playerId;

    public bool setSelectTiles(int locX, int locY, int playerId, int value)
    {
        selectTiles[locX][locY].Add(new Tiles(playerId, value));
        if (selectTiles[locX][locY].Count > 1) return false;
        else return true;
    }

    public void GameEndButton()
    {
        GameObject play_pannel = GameObject.Find("Canvas");
        play_pannel.SetActive(false);
        game_result_pannel.SetActive(false);
        gameEndPannel.SetActive(true);
    }
}
