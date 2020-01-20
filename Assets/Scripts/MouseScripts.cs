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

    void Start()
    {
        ps = GameObject.FindGameObjectWithTag("Map").GetComponentInChildren<ParticleSystem>();
        ps.Stop();
        backupColor = toastMsg.GetComponent<Image>().color;
    }

    void Update()
    {
        //Ŭ���� UI���̸� �׳� return
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        
        if (Physics.Raycast(ray, out hitInfo))
        {
            GameObject ourHitObject = hitInfo.collider.transform.gameObject;
            if (ourHitObject.GetComponent<Hex>() != null && !ourHitObject.GetComponent<Hex>().thisBaseCamp && NetworkRoundManager.isMyTurn)
            {
                //Debug.Log("Raycast hit: " + ourHitObject.name);
                if (Input.GetMouseButtonDown(0))
                {
                    mr = ourHitObject.GetComponentInChildren<MeshRenderer>();
                    ps = ourHitObject.GetComponentInChildren<ParticleSystem>();
                    Debug.Log(ps);
                    ourHitObject.GetComponentInChildren<ParticleSystem>();
                    if (choice_Map == false)
                    {

                        // ���õ� ���� ������ ���õ� ���� x, y���� ���� �� ����ȯ
                        choice_Map_x = ourHitObject.GetComponent<Hex>().x;
                        choice_Map_y = ourHitObject.GetComponent<Hex>().y;
                        ps.Play();

                        int tresh = NetworkRoundManager.public_Player_Id;
                        mr.material.color = NetworkRoundManager.getMyColor(tresh);
                        choice_Map = true;
                    }
                    // if(chice_map == true) ���õ� ���� �ִٸ� ���� ���õ� x,y �� �� �� ����
                    else
                    {
                        // x, y���� ������
                        if (ourHitObject.GetComponent<Hex>().x == choice_Map_x && choice_Map_y == ourHitObject.GetComponent<Hex>().y)
                        {
                            if (ps.isPlaying == true)
                            {
                                ps.Stop();
                                ps.Clear();
                            }
                            //������� �ٽ� ��ȯ, Color�� ��ȯ�� �Ҷ����� Unity�� ����ǥ / 255f ����� ���̴»����� ǥ����
                            int occTileId = GameObject.Find("EventSystem").GetComponent<EventManager>().getOccTiles(choice_Map_x, choice_Map_y);
                            mr.material.color = NetworkRoundManager.getMyColor(occTileId);

                            choice_Map = false;
                        }
                        // x, y���� �ٸ���
                        else
                        {
                            if (isMyMsg) StartCoroutine("MsgNotice");
                        }
                    }
                }

                //if (Input.GetMouseButton(1))
                //{
                //    return;
                //    // locX, locY, �г�.setActive(true), 
                //    //����Ǿߵ� �Լ� : �г�==> locX, locY(ȭ�� �ؽ�Ʈ ��ġǥ�� �뵵), 
                //}
            }
        }
    }

    IEnumerator MsgNotice()
    {
        isMyMsg = false;

        Debug.Log("�ϳ��� ���� ���ð����մϴ�!");
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
