using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

public class MouseScripts : MonoBehaviour
{
    public static bool choice_Map = false;
    public static int choice_Map_x = 0;
    public static int choice_Map_y = 0;
    public static MeshRenderer mr;
    public static ParticleSystem ps;
    public GameObject toastMsg;
    Color backupColor;
    public bool isMyMsg = true;

    //교전창 관련 변수
    public GameObject result_pannel;
    public GameObject War_prefab;
    // layMask로 Map타일을 제외하고 Raycast 안되게 처리하기 위한 변수
    int _layerMask;
    void Start()
    {
        _layerMask = 1 << LayerMask.NameToLayer("Map");
        ps = GameObject.FindGameObjectWithTag("Map").GetComponentInChildren<ParticleSystem>();
        ps.Stop();
        backupColor = toastMsg.GetComponent<Image>().color;
    }

    void Update()
    {
        //클릭이 UI위이면 그냥 return
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo, _layerMask))
        {
            GameObject ourHitObject = hitInfo.collider.transform.gameObject;
            if (ourHitObject.GetComponent<Hex>() != null && !ourHitObject.GetComponent<Hex>().thisBaseCamp && NetworkRoundManager.isMyTurn)
            {
                //Debug.Log("Raycast hit: " + ourHitObject.name);
                if (Input.GetMouseButtonDown(0))
                {
                    //mr = ourHitObject.GetComponentInChildren<MeshRenderer>();
                    //ps = ourHitObject.GetComponentInChildren<ParticleSystem>();
                    //Debug.Log(ps);
                    ourHitObject.GetComponentInChildren<ParticleSystem>();
                    if (choice_Map == false)
                    {
                        mr = ourHitObject.GetComponentInChildren<MeshRenderer>();
                        ps = ourHitObject.GetComponentInChildren<ParticleSystem>();
                        // 선택된 땅이 없으면 선택된 땅의 x, y값을 저장 후 색변환
                        choice_Map_x = ourHitObject.GetComponent<Hex>().x;
                        choice_Map_y = ourHitObject.GetComponent<Hex>().y;
                        ps.Play();

                        int tresh = NetworkRoundManager.public_Player_Id;
                        mr.material.color = NetworkRoundManager.getMyColor(tresh);
                        choice_Map = true;
                    }
                    // if(chice_map == true) 선택된 땅이 있다면 이전 선택된 x,y 와 비교 후 진행
                    else
                    {
                        // x, y값이 같으면
                        if (ourHitObject.GetComponent<Hex>().x == choice_Map_x && choice_Map_y == ourHitObject.GetComponent<Hex>().y)
                        {
                            if (ps.isPlaying == true)
                            {
                                ps.Stop();
                                ps.Clear();
                            }
                            //녹색으로 다시 변환, Color로 변환을 할때에는 Unity에 색상표 / 255f 해줘야 보이는색으로 표현됨
                            int occTileId = GameObject.Find("EventSystem").GetComponent<EventManager>().getOccTiles(choice_Map_x, choice_Map_y);
                            mr.material.color = NetworkRoundManager.getMyColor(occTileId);

                            choice_Map = false;
                        }
                        // x, y값이 다르면
                        else
                        {
                            if (isMyMsg) StartCoroutine("MsgNotice");
                        }
                    }
                }

                if (Input.GetMouseButtonDown(1))
                {
                    // selectTiles x,y좌표확인후 0초과면 
                    // playerid, cardid
                    // locX, locY, 패널.setActive(true), 
                    //실행되야될 함수 : 패널==> locX, locY(화면 텍스트 위치표시 용도), 
                    result_pannel.SetActive(true);
                    //GameObject _warUiGo = Instantiate(War_prefab) as GameObject;
                    Instantiate(War_prefab).transform.SetParent(GameObject.Find("Content").transform, false);
                    
                    //_warUiGo.transform.SetParent(result_pannel.transform, false);
                    //Instantiate(War_prefab, new Vector3(1, 1, 1), War_prefab.transform.transform.rotation, go.transform);


                }
                else if (Input.GetMouseButtonUp(1))
                {
                    result_pannel.SetActive(false);
                }
            }
        }
    }

    IEnumerator MsgNotice()
    {
        isMyMsg = false;

        Debug.Log("하나의 땅만 선택가능합니다!");
        toastMsg.SetActive(true);

        toastMsg.GetComponent<Image>().color = backupColor;
        Color fadeColor = toastMsg.GetComponent<Image>().color;
        float time = 0f, start = 1f, end = 0f, FadeTime = 1.1f;

        while(fadeColor.a > 0f)
        {
            time += Time.deltaTime / FadeTime;
            fadeColor.a = Mathf.Lerp(start, end, time);
            toastMsg.GetComponent<Image>().color = fadeColor;
            yield return null;
        }

        toastMsg.SetActive(false);

        isMyMsg = true;
        yield return 0;
    }
}
