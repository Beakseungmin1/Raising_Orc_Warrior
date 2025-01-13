using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 해상도 고정 카메라 스크립트
public class CameraResolution : MonoBehaviour
{
    void Start()
    {
        Camera camera = GetComponent<Camera>();
        Rect rect = camera.rect;
        float targetAspect = 9f / 16f; // 목표 비율
        float screenAspect = (float)Screen.width / Screen.height; // 실제 기기 화면 비율
        float scaleHeight = screenAspect / targetAspect;
        float scaleWidth = 1f / scaleHeight;

        // 카메라의 위치를 저장
        Vector3 originalPosition = camera.transform.position;

        // 위 아래 공백 생성 (휴대폰이 날씬한 경우)
        if (scaleHeight < 1)
        {
            rect.height = scaleHeight;
            rect.y = 0; // 공백을 위쪽으로만 배치
            // 카메라를 아래로 이동
            camera.transform.position = new Vector3(originalPosition.x, originalPosition.y - (1f - scaleHeight) / 2f, originalPosition.z);
        }
        // 좌 우 공백 생성 (휴대폰이 뚱뚱한 경우)
        else
        {
            rect.width = scaleWidth;
            rect.x = (1f - scaleWidth) / 2f;
        }

        camera.rect = rect;
    }

    void OnPreCull() => GL.Clear(true, true, Color.black);
}
