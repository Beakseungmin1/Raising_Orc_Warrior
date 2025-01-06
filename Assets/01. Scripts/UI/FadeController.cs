using System;
using System.Collections;
using UnityEngine;

public class FadeController : MonoBehaviour // Panel 불투명도 조절해 페이드인 or 페이드아웃
{
    public bool isFadeIn = false; //밝아지는거
    public bool isFadeOut = false; //어두워지는 거
    public bool isFadeOutFadeIn = false; //어두워졌다가 밝아지는 거
    public GameObject panel; // 불투명도를 조절할 Panel 오브젝트
    private Action onCompleteCallback; // FadeIn 또는 FadeOut 다음에 진행할 함수

    void Start()
    {
        if (!panel)
        {
            Debug.LogError("Panel 오브젝트를 찾을 수 없습니다.");
            throw new MissingComponentException();
        }

        if (isFadeIn) // Fade In Mode -> 바로 코루틴 시작
        {
            panel.SetActive(true); // Panel 활성화
            StartCoroutine(CoFadeIn());
        }
        else if (isFadeOut)
        {
            panel.SetActive(false); // Panel 비활성화
            FadeOut();
        }
        else // isFadeOutFadeIn
        {
            panel.SetActive(false); // Panel 비활성화
            FadeOutFadeIn();
        }
    }

    public void FadeOut()
    {
        panel.SetActive(true); // Panel 활성화
        StartCoroutine(CoFadeOut());
    }

    public void FadeOutFadeIn()
    {
        panel.SetActive(true); // Panel 활성화
        StartCoroutine(CoFadeOutFadeIn());
    }

    IEnumerator CoFadeIn()
    {
        float elapsedTime = 0f; // 누적 경과 시간
        float fadedTime = 2f; // 총 소요 시간

        while (elapsedTime <= fadedTime)
        {
            panel.GetComponent<CanvasRenderer>().SetAlpha(Mathf.Lerp(1f, 0f, elapsedTime / fadedTime));

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        panel.SetActive(false); // Panel을 비활성화
        onCompleteCallback?.Invoke(); // 이후에 해야 하는 다른 액션이 있는 경우(null이 아님) 진행한다
        Destroy(gameObject);
    }

    IEnumerator CoFadeOut()
    {
        float elapsedTime = 0f; // 누적 경과 시간
        float fadedTime = 2f; // 총 소요 시간

        while (elapsedTime <= fadedTime)
        {
            panel.GetComponent<CanvasRenderer>().SetAlpha(Mathf.Lerp(0f, 1f, elapsedTime / fadedTime));

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        onCompleteCallback?.Invoke(); // 이후에 해야 하는 다른 액션이 있는 경우(null이 아님) 진행한다
        Destroy(gameObject);
    }

    IEnumerator CoFadeOutFadeIn()
    {
        float elapsedTimeForFadeOut = 0f; // 누적 경과 시간
        float fadedTimeForFadeOut = 0.5f; // 총 소요 시간

        while (elapsedTimeForFadeOut <= fadedTimeForFadeOut)
        {
            panel.GetComponent<CanvasRenderer>().SetAlpha(Mathf.Lerp(0f, 1.2f, elapsedTimeForFadeOut / fadedTimeForFadeOut));

            elapsedTimeForFadeOut += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(0.6f);

        float elapsedTimeForFadeIn = 0f; // 누적 경과 시간
        float fadedTimeForFadeIn = 2f; // 총 소요 시간

        //페이드 인(밝아지는 것)
        while (elapsedTimeForFadeIn <= fadedTimeForFadeIn)
        {
            panel.GetComponent<CanvasRenderer>().SetAlpha(Mathf.Lerp(1f, 0f, elapsedTimeForFadeIn / fadedTimeForFadeIn));

            elapsedTimeForFadeIn += Time.deltaTime;
            yield return null;
        }
        panel.SetActive(false); // Panel을 비활성화
        onCompleteCallback?.Invoke(); // 이후에 해야 하는 다른 액션이 있는 경우(null이 아님) 진행한다
        Destroy(gameObject);
    }


    public void RegisterCallback(Action callback) // 다른 스크립트에서 콜백 액션 등록하기 위해 사용
    {
        onCompleteCallback = callback;
    }
}