using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class MouseScripts : MonoBehaviour
{
    public static bool choice_Map = false;
    public static int choice_Map_x = 0;
    public static int choice_Map_y = 0;
    public static MeshRenderer mr;
    public static ParticleSystem ps;
    void Start()
    {
        ps = GameObject.FindGameObjectWithTag("Map").GetComponentInChildren<ParticleSystem>();
        ps.Stop();
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
            if (ourHitObject.GetComponent<Hex>() != null)
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
                        switch (tresh)
                        {
                            case 1:
                                {
                                    mr.material.color = new Color(255 / 255f, 0, 0);
                                    break;
                                }
                            case 2:
                                {
                                    mr.material.color = new Color(83 / 255f, 147 / 255f, 224 / 255f);
                                    break;
                                }
                            case 3:
                                {
                                    mr.material.color = new Color(248 / 255f, 215 / 255f, 0);
                                    break;
                                }
                            case 4:
                                {
                                    mr.material.color = new Color(168 / 255f, 0 / 255f, 255 / 255f);
                                    break;
                                }
                        }
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
                            mr.material.color = new Color(32 / 255f, 84 / 255f, 30 / 255f);
                            choice_Map = false;
                        }
                        // x, y���� �ٸ���
                        else
                        {
                            Debug.Log("�ϳ��� ���� ���ð����մϴ�!");
                        }
                    }
                }
            }
        }
    }

}
