using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    #region 타일관련 변수
    //이벤트를 처리해야할 내용을 다음 selectTiles와 최종 점령지를 나타내고 있는 occtiles
    private static List<List<List<Tiles>>> selectTiles = new List<List<List<Tiles>>>();
    private static int[,] occTiles = new int[15, 8];

    //타일 2차원배열에 접근하기 위해서 현재 지정된 좌표가 어디인지 확인하기 위해서 선택된 X,Y좌표를 저장
    public static List<int> tileLocX = new List<int>();
    public static List<int> tileLocY = new List<int>();

    public static int[] player_count = new int[4] { 0, 0, 0, 0 }; //점령지 수

    private int LocX, LocY;
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

    public static void setOccTiles(int locX, int locY, int playerId)
    {
        occTiles[locX, locY] = playerId;
    }

    public static void setSelectTiles(int locX, int locY, int playerId, int value)
    {
        selectTiles[locX][locY].Add(new Tiles(playerId, value));
    }
}
