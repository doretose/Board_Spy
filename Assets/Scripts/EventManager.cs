using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    #region 타일관련 변수
    //이벤트를 처리해야할 내용을 다음 selectTiles와 최종 점령지를 나타내고 있는 occtiles
    public static List<List<List<Tiles>>> selectTiles = new List<List<List<Tiles>>>();
    public static int[,] occTiles = new int[15, 8];

    public static List<int> tileLocX = new List<int>();
    public static List<int> tileLocY = new List<int>();
    

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
                Debug.Log(j+1 + "번째 선택좌표 : (" + LocX + ", " + LocY + ") playerId, CardId = " + selectTiles[LocX][LocY][j].playerId + ",  " + selectTiles[LocX][LocY][j].cardId);
                Debug.Log($"{occTiles[LocX, LocY]}plyer 점령 땅 : ({LocX} , {LocY})");
            }
        }
    }
}
