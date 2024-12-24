using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TitleText : MonoBehaviour
{
    public TextMeshProUGUI textMeshPro;
    public float changeSpeed = 1f; // 색상 변경 속도

    private void Start()
    {
        StartCoroutine(ChangeTextColor());
    }

    private IEnumerator ChangeTextColor()
    {
        while (true)
        {
            // 현재 시간에 따라 색상 계산
            float t = Mathf.PingPong(Time.time * changeSpeed, 1);
            Color color = Color.HSVToRGB(t, 1, 1); // HSV 색상 모델을 사용하여 무지개색 생성

            // 텍스트 색상 변경
            textMeshPro.color = color;

            yield return null; // 다음 프레임까지 대기
        }
    }
}
