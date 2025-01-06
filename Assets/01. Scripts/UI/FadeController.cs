using System;
using System.Collections;
using UnityEngine;

public class FadeController : MonoBehaviour // Panel ������ ������ ���̵��� or ���̵�ƿ�
{
    public bool isFadeIn = false; //������°�
    public bool isFadeOut = false; //��ο����� ��
    public bool isFadeOutFadeIn = false; //��ο����ٰ� ������� ��
    public GameObject panel; // �������� ������ Panel ������Ʈ
    private Action onCompleteCallback; // FadeIn �Ǵ� FadeOut ������ ������ �Լ�

    void Start()
    {
        if (!panel)
        {
            Debug.LogError("Panel ������Ʈ�� ã�� �� �����ϴ�.");
            throw new MissingComponentException();
        }

        if (isFadeIn) // Fade In Mode -> �ٷ� �ڷ�ƾ ����
        {
            panel.SetActive(true); // Panel Ȱ��ȭ
            StartCoroutine(CoFadeIn());
        }
        else if (isFadeOut)
        {
            panel.SetActive(false); // Panel ��Ȱ��ȭ
            FadeOut();
        }
        else // isFadeOutFadeIn
        {
            panel.SetActive(false); // Panel ��Ȱ��ȭ
            FadeOutFadeIn();
        }
    }

    public void FadeOut()
    {
        panel.SetActive(true); // Panel Ȱ��ȭ
        StartCoroutine(CoFadeOut());
    }

    public void FadeOutFadeIn()
    {
        panel.SetActive(true); // Panel Ȱ��ȭ
        StartCoroutine(CoFadeOutFadeIn());
    }

    IEnumerator CoFadeIn()
    {
        float elapsedTime = 0f; // ���� ��� �ð�
        float fadedTime = 2f; // �� �ҿ� �ð�

        while (elapsedTime <= fadedTime)
        {
            panel.GetComponent<CanvasRenderer>().SetAlpha(Mathf.Lerp(1f, 0f, elapsedTime / fadedTime));

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        panel.SetActive(false); // Panel�� ��Ȱ��ȭ
        onCompleteCallback?.Invoke(); // ���Ŀ� �ؾ� �ϴ� �ٸ� �׼��� �ִ� ���(null�� �ƴ�) �����Ѵ�
        Destroy(gameObject);
    }

    IEnumerator CoFadeOut()
    {
        float elapsedTime = 0f; // ���� ��� �ð�
        float fadedTime = 2f; // �� �ҿ� �ð�

        while (elapsedTime <= fadedTime)
        {
            panel.GetComponent<CanvasRenderer>().SetAlpha(Mathf.Lerp(0f, 1f, elapsedTime / fadedTime));

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        onCompleteCallback?.Invoke(); // ���Ŀ� �ؾ� �ϴ� �ٸ� �׼��� �ִ� ���(null�� �ƴ�) �����Ѵ�
        Destroy(gameObject);
    }

    IEnumerator CoFadeOutFadeIn()
    {
        float elapsedTimeForFadeOut = 0f; // ���� ��� �ð�
        float fadedTimeForFadeOut = 0.5f; // �� �ҿ� �ð�

        while (elapsedTimeForFadeOut <= fadedTimeForFadeOut)
        {
            panel.GetComponent<CanvasRenderer>().SetAlpha(Mathf.Lerp(0f, 1.2f, elapsedTimeForFadeOut / fadedTimeForFadeOut));

            elapsedTimeForFadeOut += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(0.6f);

        float elapsedTimeForFadeIn = 0f; // ���� ��� �ð�
        float fadedTimeForFadeIn = 2f; // �� �ҿ� �ð�

        //���̵� ��(������� ��)
        while (elapsedTimeForFadeIn <= fadedTimeForFadeIn)
        {
            panel.GetComponent<CanvasRenderer>().SetAlpha(Mathf.Lerp(1f, 0f, elapsedTimeForFadeIn / fadedTimeForFadeIn));

            elapsedTimeForFadeIn += Time.deltaTime;
            yield return null;
        }
        panel.SetActive(false); // Panel�� ��Ȱ��ȭ
        onCompleteCallback?.Invoke(); // ���Ŀ� �ؾ� �ϴ� �ٸ� �׼��� �ִ� ���(null�� �ƴ�) �����Ѵ�
        Destroy(gameObject);
    }


    public void RegisterCallback(Action callback) // �ٸ� ��ũ��Ʈ���� �ݹ� �׼� ����ϱ� ���� ���
    {
        onCompleteCallback = callback;
    }
}