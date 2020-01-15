using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScripts : MonoBehaviour
{
    float zoomSpeed = 4.0f;
    //public float rotateSpeed = 10.0f;
    private Camera mainCamera;
    float move_max_distance = 10;
    float move_min_distance = -3;

    float zoom_max_distance = 4;
    float zoom_min_distance = -6;
    float current_distance = 0;
    //float rotate_max = 100;
    //float rotate_min = 45;
    private float dist;
    Vector3 lastDragPosition;

    void Start()
    {
        mainCamera = GetComponent<Camera>();
        dist = transform.position.z;
    }

    void Update()
    {
        UpdateDrag();
        Zoom();
        //Rotate();
    }

    void UpdateDrag()
    {
        if (Input.GetMouseButtonDown(0))
            lastDragPosition = Input.mousePosition;
        if (Input.GetMouseButton(0))
        {
            var delta = lastDragPosition - Input.mousePosition;
            if (mainCamera.transform.position.x <= 0)
            {
                if (delta.x >= 0) Move_Camera(delta);
                else return;
            }
            else if (mainCamera.transform.position.x >= 26)
            {
                if (delta.x < 0) Move_Camera(delta);
                else return;
            }
            else if (mainCamera.transform.position.z <= move_min_distance)
            {
                if (delta.y >= 0) Move_Camera(delta);
                else return;
            }
            else if (mainCamera.transform.position.z >= move_max_distance)
            {
                if (delta.y < 0) Move_Camera(delta);
                else return;
            }
            else
                Move_Camera(delta);
        }
    }

    private void Move_Camera(Vector3 delta)
    {
        transform.Translate(delta * Time.deltaTime * 1.2f);
        lastDragPosition = Input.mousePosition;
    }

    private void Zoom()
    {
        float distance = Input.GetAxis("Mouse ScrollWheel") * -1 * zoomSpeed;
        
        if (distance != 0)
        {
            if (current_distance >= zoom_max_distance)
            {
                if (distance <= 0)
                {
                    mainCamera.orthographicSize += distance;
                    current_distance += distance;
                }
                else return;
            }
            else if(current_distance <= zoom_min_distance)
            {
                if (distance >= 0)
                {
                    mainCamera.orthographicSize += distance;
                    current_distance += distance;
                }
                else return;
            }
            else
            {
                mainCamera.orthographicSize += distance;
                current_distance += distance;
            }
        }
    }
    //private void Rotate()
    //{
    //    if (Input.GetMouseButton(1))
    //    {
    //        Vector3 rot = transform.rotation.eulerAngles; // 현재 카메라의 각도를 Vector3로 반환
    //        rot.x += -1 * Input.GetAxis("Mouse Y") * rotateSpeed; // 마우스 Y 위치 * 회전 스피드
    //        Quaternion q = Quaternion.Euler(rot); // Quaternion으로 변환
    //        q.z = 0;
    //        transform.rotation = Quaternion.Slerp(transform.rotation, q, 2f); // 자연스럽게 회전
    //    }
    //}
}