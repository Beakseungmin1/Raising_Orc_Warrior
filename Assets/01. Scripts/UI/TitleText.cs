using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TitleText : MonoBehaviour
{
    public TextMeshProUGUI textMeshPro;
    public float changeSpeed = 1f; // ���� ���� �ӵ�

    private void Start()
    {
        StartCoroutine(ChangeTextColor());
    }

    private IEnumerator ChangeTextColor()
    {
        while (true)
        {
            // ���� �ð��� ���� ���� ���
            float t = Mathf.PingPong(Time.time * changeSpeed, 1);
            Color color = Color.HSVToRGB(t, 1, 1); // HSV ���� ���� ����Ͽ� �������� ����

            // �ؽ�Ʈ ���� ����
            textMeshPro.color = color;

            yield return null; // ���� �����ӱ��� ���
        }
    }
}
