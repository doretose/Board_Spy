﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefebManager : MonoBehaviour
{
    public static void CreateArrowPrefeb(GameObject isPrefeb, int locX, int locY)
    {
        
        GameObject go = GameObject.Find(locX + ", " + locY);
        MeshRenderer mr = go.GetComponent<MeshRenderer>();
        float pos_x = go.transform.position.x;
        float pos_z = go.transform.position.z;
        GameObject arrow_prefab = Instantiate(isPrefeb, new Vector3(pos_x, 1, pos_z-1), isPrefeb.transform.rotation);
        Destroy(arrow_prefab, 3f);
    }


    public static void CreatePrefeb(GameObject isPrefeb, int locX, int locY, Color isColor)
    {
        GameObject go = GameObject.Find(locX + ", " + locY);
        MeshRenderer mr = go.GetComponent<MeshRenderer>();
        float pos_x = go.transform.position.x;
        float pos_z = go.transform.position.z;
        
        Instantiate(isPrefeb, new Vector3(pos_x, 1, pos_z), isPrefeb.transform.transform.rotation, go.transform);
        mr.material.color = isColor;
    }

    public static void CreatePrefeb(GameObject isPrefeb, int locX, int locY, GameObject choice_effect)
    {
        GameObject go = GameObject.Find(locX + ", " + locY);
        float pos_x = go.transform.position.x;
        float pos_z = go.transform.position.z;
        //choice_effect는 펑하고 터지는 효과 이펙트
        Destroy(Instantiate(choice_effect, new Vector3(pos_x, 1, pos_z), choice_effect.transform.transform.rotation, go.transform), 2f);
        Instantiate(isPrefeb, new Vector3(pos_x, 1, pos_z), isPrefeb.transform.transform.rotation, go.transform);
    }

    public static void CreateSwordPrefeb(GameObject isPrefeb, int locX, int locY, GameObject choice_effect)
    {
        SoundManager.instance.PlayswordSound();
        GameObject go = GameObject.Find(locX + ", " + locY);
        float pos_x = go.transform.position.x;
        float pos_z = go.transform.position.z;
        Destroy(Instantiate(choice_effect, new Vector3(pos_x, 1, pos_z), choice_effect.transform.transform.rotation, go.transform), 2f);
        pos_x += (float)0.2;
        Instantiate(isPrefeb, new Vector3(pos_x, 1, pos_z), isPrefeb.transform.transform.rotation, go.transform);
    }



    public static void DestroyPrefebs(int locX, int locY)
    {
        //색깔 초록색으로 변경 해당 타일위의 프리팹을 모두 삭제
        GameObject endTile = GameObject.Find(locX + ", " + locY);
        //MeshRenderer mr = endTile.GetComponent<MeshRenderer>();
        //mr.material.color = new Color(32 / 255f, 84 / 255f, 30 / 255f);

        //탐색이 끝난 타일 위에 놓인 모든 기물 프리팹을 제거
        Transform[] endTileChilds;
        endTileChilds = endTile.GetComponentsInChildren<Transform>(true);

        for (int j = 7; j < endTileChilds.Length; ++j)
        {
            Destroy(endTileChilds[j].gameObject);
        }
    }
}
