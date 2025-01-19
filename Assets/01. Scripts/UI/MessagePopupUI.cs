using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class MessagePopupUI : UIBase
{
    [SerializeField] private GameObject messageObj;
    [SerializeField] private TextMeshProUGUI messageTxt;
    [SerializeField] private float displayDuration = 0.5f;
    [SerializeField] private float fadeInDuration = 0.25f;
    [SerializeField] private float fadeOutDuration = 0.25f;

    private Coroutine displayCoroutine;

    private void Awake()
    {
        GameEventsManager.Instance.messageEvents.onShowMessage += ShowMessage;
    }

    private void Start()
    {
        messageObj.SetActive(false);
    }

    public void ShowMessage(MessageTextType messageType, float duration, int sortingOrder)
    {
        if (!gameObject.activeInHierarchy)
        {
            gameObject.SetActive(true);
        }

        displayDuration = duration; //0.5f;
        fadeInDuration = duration * 0.5f; //0.25f;
        fadeOutDuration = duration * 0.5f; //0.25f;

        switch (messageType)
        {
            case MessageTextType.Equipped:
                messageTxt.text = "장착을 완료했습니다.";
                break;
            case MessageTextType.DungeonEntryBlocked:
                messageTxt.text = "전투 중 던전 입장이 불가합니다.";
                break ;
            case MessageTextType.DungeonTicketNotEnough:
                messageTxt.text = "던전 티켓이 부족합니다.";
                break;
            case MessageTextType.TakeQuestReward:
                messageTxt.text = "퀘스트 보상을 받았습니다.";
                break;
            default:
                break;
        }

        if (displayCoroutine != null)
        {
            StopCoroutine(displayCoroutine);
        }
        displayCoroutine = StartCoroutine(DisplayMessageCoroutine(sortingOrder));
    }

    private IEnumerator DisplayMessageCoroutine(int sortOrder)
    {
        messageObj.SetActive(true);

        //소팅 오더 설정 
        int savedSortingOrder = messageObj.GetComponent<MessagePopupUI>().canvas.sortingOrder;
        messageObj.GetComponent<MessagePopupUI>().canvas.sortingOrder = sortOrder;

        SetMessageAlpha(0f);
        yield return FadeInMessage();
        yield return new WaitForSeconds(displayDuration);
        yield return FadeOutMessage();
        messageObj.SetActive(false);
        displayCoroutine = null;

        //소팅오더 원상복구
        messageObj.GetComponent<MessagePopupUI>().canvas.sortingOrder = savedSortingOrder;
    }

    private void SetMessageAlpha(float alpha)
    {
        SpriteRenderer spriteRenderer = messageObj.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, alpha);
        }
        else
        {
            Image image = messageObj.GetComponent<Image>();
            if (image != null)
            {
                image.color = new Color(image.color.r, image.color.g, image.color.b, alpha);
            }
        }
    }

    private IEnumerator FadeInMessage()
    {
        float elapsedTime = 0f;
        float targetAlpha = 0.6f;
        while (elapsedTime < fadeInDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, targetAlpha, elapsedTime / fadeInDuration);
            SetMessageAlpha(alpha);
            yield return null;
        }
        SetMessageAlpha(targetAlpha);
    }

    private IEnumerator FadeOutMessage()
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeOutDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(0.6f, 0f, elapsedTime / fadeOutDuration);
            SetMessageAlpha(alpha);
            yield return null;
        }
        SetMessageAlpha(0f);
    }
}