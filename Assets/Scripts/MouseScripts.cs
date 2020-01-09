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
        //클릭이 UI위이면 그냥 return
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

                        // 선택된 땅이 없으면 선택된 땅의 x, y값을 저장 후 색변환
                        choice_Map_x = ourHitObject.GetComponent<Hex>().x;
                        choice_Map_y = ourHitObject.GetComponent<Hex>().y;
                        ps.Play();
                        mr.material.color = Color.red;
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
                            mr.material.color = new Color(32 / 255f, 84 / 255f, 30 / 255f);
                            choice_Map = false;
                        }
                        // x, y값이 다르면
                        else
                        {
                            Debug.Log("하나의 땅만 선택가능합니다!");
                        }
                    }
                }
            }
        }
    }

}
