using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetCamera : MonoBehaviour
{
    Camera cam;

    private void Awake()
    {
        cam = GetComponent<Camera>();
    }

    public void SetCameraPosY(float posY)
    {
        Vector3 newPos = transform.position;
        newPos.y = posY;
        transform.position = newPos;
    }

    public void SetCameraSize(float size)
    {
        cam.orthographicSize = size;
    }
}
