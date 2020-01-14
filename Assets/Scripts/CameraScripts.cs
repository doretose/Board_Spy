using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScripts : MonoBehaviour
{
    public float rotateSpeed = 10.0f;
    public float zoomSpeed = 10.0f;

    private Camera mainCamera;
    float max_distance = 10;
    float min_distance = -10;
    float current_distance = 0;

    private float dist;
    private Vector3 MouseStart;
    private Vector3 derp;
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
    }

    void UpdateDrag()
    {
        if (Input.GetMouseButtonDown(0))
            lastDragPosition = Input.mousePosition;
        if (Input.GetMouseButton(0))
        {
            var delta = lastDragPosition - Input.mousePosition;
            transform.Translate(delta * Time.deltaTime * 0.8f);
            lastDragPosition = Input.mousePosition;
        }
    }

    private void Zoom()
    {
        
        float distance = Input.GetAxis("Mouse ScrollWheel") * -1 * zoomSpeed;
        if (distance != 0)
        {
            Debug.Log(current_distance);
            if (current_distance >= max_distance)
            {
                if (distance <= 0)
                {
                    mainCamera.fieldOfView += distance;
                    current_distance += distance;
                }
                else return;
            }
            else if(current_distance <= min_distance)
            {
                if (distance >= 0)
                {
                    mainCamera.fieldOfView += distance;
                    current_distance += distance;
                }
                else return;
            }
            else
            {
                mainCamera.fieldOfView += distance;
                current_distance += distance;
            }
        }
        
    }
}