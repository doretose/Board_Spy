using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    #region 타일관련 변수
    //이벤트를 처리해야할 내용을 다음 selectTiles와 최종 점령지를 나타내고 있는 occtiles
    private List<List<List<Tiles>>> selectTiles = new List<List<List<Tiles>>>();
    private int[,] occTiles = new int[15, 8];

    //타일 2차원배열에 접근하기 위해서 현재 지정된 좌표가 어디인지 확인하기 위해서 선택된 X,Y좌표를 저장
    public static List<int> tileLocX = new List<int>();
    public static List<int> tileLocY = new List<int>();

    public static int[] player_count = new int[4] { 0, 0, 0, 0 }; //점령지 수

    //타일 순회를 위한 x,y좌표 저장 변수
    private int LocX, LocY;

    //깃발 프리팹
    public GameObject[] flag = new GameObject[4];
    #endregion

    // 3차원 배열 selectTiles 초기화
    private void Awake()
    {
        for (int i = 0; i < 15; i++)
        {
            selectTiles.Add(new List<List<Tiles>>());
            for (int j = 0; j < 8; j++)
            {
                selectTiles[i].Add(new List<Tiles>());
            }
        }
    }

    // 타일 위치에 관한 이벤트 처리
    void Update()
    {

        for (int i = 0; i < tileLocX.Count; ++i)
        {
            LocX = tileLocX[i];
            LocY = tileLocY[i];
            for (int j = 0; j < selectTiles[LocX][LocY].Count; ++j)
            {
                //Debug.Log(j+1 + "번째 선택좌표 : (" + LocX + ", " + LocY + ") playerId, CardId = " + selectTiles[LocX][LocY][j].playerId + ",  " + selectTiles[LocX][LocY][j].cardId);
                //Debug.Log($"{occTiles[LocX, LocY]}plyer 점령 땅 : ({LocX} , {LocY})");
            }
        }

        player_count = new int[4] {0,0,0,0};
        for (int i = 0; i < occTiles.GetLength(0); i++)
        {
            for (int j = 0; j < occTiles.GetLength(1); j++)
            {
                if (occTiles[i, j] != 0)
                {
                    player_count[occTiles[i, j]-1] += 1;
                }
            }
        }
        //Debug.Log($"점령지 1plyer : {player_count[0]} , 2player : {player_count[1]}, 3player : {player_count[2]}, 4player : {player_count[3]}");
    }

    public void RoundResult()
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
            for (int j = 0; j < selectTiles[LocX][LocY].Count; ++j)
            {
                checkPlayer = selectTiles[LocX][LocY][j].playerId;
                checkCard = selectTiles[LocX][LocY][j].cardId;

                switch (checkCard)
                {
                    case 1: //점령
                        {
                            resultTiles.Add(new Tiles(checkPlayer, checkCard));
                            break;
                        }
                    case 2: //방어
                        {
                            if(occTiles[LocX, LocY] == NetworkRoundManager.public_Player_Id)
                            {
                                int idx = 0;
                                while (idx < resultTiles.Count)
                                {
                                    if (resultTiles[idx].playerId != NetworkRoundManager.public_Player_Id)
                                    {
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
                        {
                            break;
                        }
                }
            }

            selectTiles[LocX][LocY].Clear();
            //색깔 초록색으로 변경 해당 타일위의 프리팹을 모두 삭제
            GameObject endTile = GameObject.Find(LocX + ", " + LocY);
            MeshRenderer mr = endTile.GetComponent<MeshRenderer>();
            mr.material.color = new Color(32 / 255f, 84 / 255f, 30 / 255f);

            //GameObject[] endTileChilds;
            //endTileChilds = endTile.GetComponentsInChildren<GameObject>();

            //foreach (GameObject endTileChild in endTileChilds)
            //{
            //    if(endTileChild.name != "CFX_LightWall 1") Destroy(endTileChild);
            //}

            Debug.Log($"({LocX} , {LocY})의 resultTiles의 수 {resultTiles.Count}");
            if (resultTiles.Count > 0)
            {
                Debug.Log($"({LocX} , {LocY}) = {resultTiles[0].playerId}");
                occPlayerId = resultTiles[0].playerId;
            }
            if (occPlayerId != 0)
            {
                occTiles[LocX, LocY] = occPlayerId;
                PrefebManager.CreatePrefeb(flag[occPlayerId - 1], LocX, LocY, NetworkRoundManager.getMyColor(occPlayerId));
            }
        }

        //최종적으로 player_count를 재탐색하고 타일 초기화 -> occTile에 따른 결과 반영
        player_count = new int[4] { 0, 0, 0, 0 };
        for (int i = 0; i < occTiles.GetLength(0); i++)
        {
            for (int j = 0; j < occTiles.GetLength(1); j++)
            {
                if (occTiles[i, j] != 0)
                {
                    player_count[occTiles[i, j] - 1] += 1;
                }
            }
        }

        tileLocX.Clear();
        tileLocY.Clear();
    }

    public void setOccTiles(int locX, int locY, int playerId)
    {
        occTiles[locX, locY] = playerId;
    }

    public void setSelectTiles(int locX, int locY, int playerId, int value)
    {
        selectTiles[locX][locY].Add(new Tiles(playerId, value));
    }

    public int getOccTiles(int locX, int locY)
    {
        return occTiles[locX, locY];
    }
}
