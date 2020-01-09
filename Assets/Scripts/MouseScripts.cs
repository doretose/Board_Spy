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
                        mr.material.color = Color.red;
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
