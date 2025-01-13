using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �ػ� ���� ī�޶� ��ũ��Ʈ
public class CameraResolution : MonoBehaviour
{
    void Start()
    {
        Camera camera = GetComponent<Camera>();
        Rect rect = camera.rect;
        float targetAspect = 9f / 16f; // ��ǥ ����
        float screenAspect = (float)Screen.width / Screen.height; // ���� ��� ȭ�� ����
        float scaleHeight = screenAspect / targetAspect;
        float scaleWidth = 1f / scaleHeight;

        // ī�޶��� ��ġ�� ����
        Vector3 originalPosition = camera.transform.position;

        // �� �Ʒ� ���� ���� (�޴����� ������ ���)
        if (scaleHeight < 1)
        {
            rect.height = scaleHeight;
            rect.y = 0; // ������ �������θ� ��ġ
            // ī�޶� �Ʒ��� �̵�
            camera.transform.position = new Vector3(originalPosition.x, originalPosition.y - (1f - scaleHeight) / 2f, originalPosition.z);
        }
        // �� �� ���� ���� (�޴����� �׶��� ���)
        else
        {
            rect.width = scaleWidth;
            rect.x = (1f - scaleWidth) / 2f;
        }

        camera.rect = rect;
    }

    void OnPreCull() => GL.Clear(true, true, Color.black);
}
